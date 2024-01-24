// Buoyancy Toolkit
// Copyright 2016 Gustav Olsson
using UnityEngine;

namespace BuoyancyToolkit
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuoyancyForce : MonoBehaviour, IBuoyancyForce
    {
        protected static float BoundsExtentBias = 0.01f;
        protected static float WeightFactorBias = 0.0f;
        protected static int[] SampleCountAtQuality = { 3, 8, 16 };

        [SerializeField] protected Collider buoyancyCollider;
        [SerializeField] protected BuoyancyQuality quality = BuoyancyQuality.Medium;
        [SerializeField] protected int sampleCount = SampleCountAtQuality[(int)BuoyancyQuality.Medium];
        [SerializeField] protected bool useWeighting = true;
        [SerializeField] protected float weightFactor = 1.5f;
        [SerializeField] protected float dragScalar = 1.0f;
        [SerializeField] protected float angularDragScalar = 1.0f;
        [SerializeField] protected LayerMask ignoreLayers = new LayerMask();
        [SerializeField] protected bool debugVisualization = false;

        protected Rigidbody rb;
        protected float nonfluidDrag;
        protected float nonfluidAngularDrag;
        protected float[] sampleIntersections;
        protected float[] sampleVolumes;

        protected FluidVolume fluidVolume;
        protected bool isSubmerged;
        protected bool isCompletelySubmerged;
        protected float volume;
        protected float submergedVolume;
        
        public Collider BuoyancyCollider
        {
            get { return buoyancyCollider; }

            set
            {
                if (buoyancyCollider != value)
                {
                    buoyancyCollider = value;

                    UpdateState();
                }
            }
        }
        
        public BuoyancyQuality Quality
        {
            get { return quality; }

            set
            {
                if (quality != value)
                {
                    quality = (BuoyancyQuality)Mathf.Clamp((int)value, 0, 3);

                    if (quality != BuoyancyQuality.Custom)
                    {
                        SetSampleCount(SampleCountAtQuality[(int)quality]);
                    }
                }
            }
        }
        
        public int Samples
        {
            get { return sampleCount; }

            set
            {
                if (sampleCount != value)
                {
                    quality = BuoyancyQuality.Custom;

                    SetSampleCount(value);
                }
            }
        }
        
        public bool UseWeighting
        {
            get { return useWeighting; }
            set { useWeighting = value; }
        }
        
        public float WeightFactor
        {
            get { return weightFactor; }
            set { weightFactor = value; }
        }
        
        public float DragScalar
        {
            get { return dragScalar; }
            set { dragScalar = value; }
        }
        
        public float AngularDragScalar
        {
            get { return angularDragScalar; }
            set { angularDragScalar = value; }
        }
        
        public float NonfluidDrag
        {
            get { return nonfluidDrag; }
            set { nonfluidDrag = value; }
        }
        
        public float NonfluidAngularDrag
        {
            get { return nonfluidAngularDrag; }
            set { nonfluidAngularDrag = value; }
        }

        public LayerMask IgnoreLayers
        {
            get { return ignoreLayers; }
            set { ignoreLayers = value; }
        }

        public bool DebugVisualization
        {
            get { return debugVisualization; }
            set { debugVisualization = value; }
        }
        
        public FluidVolume FluidVolume
        {
            get { return fluidVolume; }
        }
        
        public bool IsSubmerged
        {
            get { return isSubmerged; }
        }
        
        public bool IsCompletelySubmerged
        {
            get { return isCompletelySubmerged; }
        }
        
        public float Volume
        {
            get { return volume; }
        }
        
        public float SubmergedVolume
        {
            get { return submergedVolume; }
        }

        protected void SetSampleCount(int count)
        {
            int validCount = Mathf.Max(1, count);

            if (validCount != sampleCount)
            {
                sampleCount = validCount;
                sampleIntersections = new float[validCount * validCount];
                sampleVolumes = new float[validCount * validCount];
                
                UpdateState();
            }
        }
        
        protected void ResetState()
        {
            isSubmerged = false;
            isCompletelySubmerged = false;
            volume = 0.0f;
            submergedVolume = 0.0f;
        }
        
        protected void UpdateState()
        {
            if (buoyancyCollider == null || fluidVolume == null)
            {
                ResetState();
                return;
            }
            
            Bounds sampleBounds;
            float sampleSizeX, sampleSizeZ;

            BoundsToValues(buoyancyCollider.bounds, out sampleBounds, out sampleSizeX, out sampleSizeZ);

            CalculateVolumeAndSubmergedVolume(sampleBounds, sampleSizeX, sampleSizeZ);
            
            SetDrag();
        }

        protected void UpdateStateAndApplyImpulses()
        {
            if (buoyancyCollider == null || fluidVolume == null)
            {
                ResetState();
                return;
            }
            
            Bounds sampleBounds;
            float sampleSizeX, sampleSizeZ;

            BoundsToValues(buoyancyCollider.bounds, out sampleBounds, out sampleSizeX, out sampleSizeZ);

            CalculateVolumeAndSubmergedVolume(sampleBounds, sampleSizeX, sampleSizeZ);

            SetDrag();

            ApplyImpulses(sampleBounds, sampleSizeX, sampleSizeZ);
        }

        protected void BoundsToValues(Bounds bounds, out Bounds sampleBounds, out float sampleSizeX, out float sampleSizeZ)
        {
            sampleBounds = bounds;
            sampleBounds.Expand(BoundsExtentBias);
            sampleSizeX = sampleBounds.size.x / sampleCount;
            sampleSizeZ = sampleBounds.size.z / sampleCount;
        }
        
        protected void CalculateVolumeAndSubmergedVolume(Bounds sampleBounds, float sampleSizeX, float sampleSizeZ)
        {
            float sampleBaseArea = sampleSizeX * sampleSizeZ;

            isSubmerged = false;
            isCompletelySubmerged = true;
            volume = 0.0f;
            submergedVolume = 0.0f;
            
            Vector3 start = sampleBounds.min;
            float height = sampleBounds.size.y;

            for (int x = 0; x < sampleCount; x++)
            {
                for (int z = 0; z < sampleCount; z++)
                {
                    int index = x * sampleCount + z;
                    Vector3 point = start + new Vector3(sampleSizeX * (x + 0.5f), 0.0f, sampleSizeZ * (z + 0.5f));

                    // Raycast from bottom of fluid volume to top
                    RaycastHit lowerHit;
                    
                    if (buoyancyCollider.Raycast(new Ray(point, Vector3.up), out lowerHit, height))
                    {
                        float fluidBox = lowerHit.distance;

                        // Raycast from top of fluid volume to bottom
                        RaycastHit upperHit;

                        if (buoyancyCollider.Raycast(new Ray(point + Vector3.up * height, Vector3.down), out upperHit, height))
                        {
                            fluidBox += upperHit.distance;
                        }

                        // Calculate sample volume
                        float sampleVolume = (height - fluidBox) * sampleBaseArea;

                        // Calculate submerged sample volume
                        float lowerHeight = lowerHit.distance;
                        float upperHeight = height - upperHit.distance;
                        
                        float surfaceHeight = fluidVolume.GetHeightAt(point) - sampleBounds.min.y;

                        float submergedHeight = 0.0f;

                        if (lowerHeight < surfaceHeight)
                        {
                            isSubmerged = true;

                            submergedHeight += surfaceHeight - lowerHeight;

                            // Is sample completely submerged?
                            if (upperHeight < surfaceHeight)
                            {
                                submergedHeight -= surfaceHeight - upperHeight;
                            }
                            else
                            {
                                isCompletelySubmerged = false;
                            }
                        }

                        float sampleSubmergedVolume = submergedHeight * sampleBaseArea;

                        // Store results
                        volume += sampleVolume;
                        submergedVolume += sampleSubmergedVolume;

                        sampleIntersections[index] = lowerHeight;
                        sampleVolumes[index] = sampleSubmergedVolume;
                    }
                    else
                    {
                        sampleIntersections[index] = 0.0f;
                        sampleVolumes[index] = -1.0f;
                    }
                }
            }

            if (!isSubmerged)
            {
                isCompletelySubmerged = false;
            }

            // No ray hit?
            if (volume <= Mathf.Epsilon)
            {
                volume = height * sampleBaseArea + Mathf.Epsilon;
            }
        }
        
        protected void SetDrag()
        {
            float drag = nonfluidDrag;
            float angularDrag = nonfluidAngularDrag;
            
            if (isSubmerged)
            {
                drag += fluidVolume.rigidbodyDrag * dragScalar;
                angularDrag += fluidVolume.rigidbodyAngularDrag * angularDragScalar;
            }
            
            rb.drag = drag;
            rb.angularDrag = angularDrag;
        }

        protected void ApplyImpulses(Bounds sampleBounds, float sampleSizeX, float sampleSizeZ)
        {
            float impulse = -Physics.gravity.y * Time.fixedDeltaTime;

            if (useWeighting)
            {
                impulse *= ((weightFactor + WeightFactorBias) * rb.mass) / volume;
            }
            else
            {
                impulse *= fluidVolume.density;
            }

            for (int x = 0; x < sampleCount; x++)
            {
                for (int z = 0; z < sampleCount; z++)
                {
                    int index = x * sampleCount + z;
                    float sampleVolume = sampleVolumes[index];

                    if (sampleVolume > 0.0f)
                    {
                        Vector3 point = sampleBounds.min + new Vector3(sampleSizeX * (x + 0.5f), 0.0f, sampleSizeZ * (z + 0.5f));

                        rb.AddForceAtPosition(Vector3.up * (sampleVolume * impulse), point + Vector3.up * sampleIntersections[index], ForceMode.Impulse);
                    }
                }
            }
        }

        public void Start()
        {
            rb = GetComponent<Rigidbody>();

            // Make sure that the sample location cache is initialized
            sampleIntersections = new float[sampleCount * sampleCount];
            sampleVolumes = new float[sampleCount * sampleCount];

            // Set initial drag values
            nonfluidDrag = rb.drag;
            nonfluidAngularDrag = rb.angularDrag;

            // Update volume
            UpdateState();
        }

        public void FixedUpdate()
        {
            rb = GetComponent<Rigidbody>();
            
            UpdateStateAndApplyImpulses();
        }
        
        public void OnDrawGizmos()
        {
            if (!debugVisualization || buoyancyCollider == null || fluidVolume == null)
            {
                return;
            }
            
            Bounds sampleBounds;
            float sampleSizeX, sampleSizeZ;
            
            BoundsToValues(buoyancyCollider.bounds, out sampleBounds, out sampleSizeX, out sampleSizeZ);
            
            // Draw bounds
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(sampleBounds.center, sampleBounds.size);

            // Draw samples
            Color hitColor = new Color(1.0f, 0.0f, 0.0f, 0.2f);
            Color missColor = new Color(0.0f, 0.0f, 1.0f, 0.2f);

            for (int x = 0; x < sampleCount; x++)
            {
                for (int z = 0; z < sampleCount; z++)
                {
                    int index = x * sampleCount + z;
                    bool hit = sampleVolumes[index] > 0.0f;
                    float height = hit ? sampleIntersections[index] : BoundsExtentBias;

                    Vector3 point = sampleBounds.min + new Vector3(sampleSizeX * (x + 0.5f), height * 0.5f, sampleSizeZ * (z + 0.5f));

                    Gizmos.color = hit ? hitColor : missColor;
                    Gizmos.DrawCube(point, new Vector3(sampleSizeX * 0.9f, height, sampleSizeZ * 0.9f));
                }
            }
        }
        
        public void OnTriggerStay(Collider collider)
        {
            if (fluidVolume == null)
            {
                FluidVolume triggerFluidVolume = collider.GetComponent<FluidVolume>();

                if (triggerFluidVolume != null)
                {
                    int triggerLayer = 1 << triggerFluidVolume.gameObject.layer;

                    if ((triggerLayer & ignoreLayers.value) == 0)
                    {
                        fluidVolume = triggerFluidVolume;
                        
                        UpdateState();
                    }
                }
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            if (fluidVolume != null)
            {
                FluidVolume triggerFluidVolume = collider.GetComponent<FluidVolume>();

                if (triggerFluidVolume != null && triggerFluidVolume == fluidVolume)
                {
                    fluidVolume = null;
                    
                    UpdateState();
                }
            }
        }
    }
}