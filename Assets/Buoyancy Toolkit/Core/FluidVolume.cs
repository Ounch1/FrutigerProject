// Buoyancy Toolkit
// Copyright 2015 Gustav Olsson
using UnityEngine;

namespace BuoyancyToolkit
{
    [RequireComponent(typeof(BoxCollider))]
    public class FluidVolume : MonoBehaviour
    {
        protected float surfaceHeight = 0.0f;

        /// <summary>
        /// The density of the fluid. Only used when weighting is disabled.
        /// </summary>
        public float density = 1000.0f;

        /// <summary>
        /// The linear drag that is applied to a rigidbody submerged in the fluid volume.
        /// </summary>
        public float rigidbodyDrag = 1.0f;

        /// <summary>
        /// The angular drag that is applied to a rigidbody submerged in the fluid volume.
        /// </summary>
        public float rigidbodyAngularDrag = 1.0f;

        public void Start()
        {
            surfaceHeight = transform.position.y;
        }

        public void Update()
        {
            surfaceHeight = transform.position.y;
        }

        /// <summary>
        /// Projects a point onto the surface of the fluid volume.
        /// </summary>
        public Vector3 ProjectPointOntoSurface(Vector3 point)
        {
            return new Vector3(point.x, GetHeightAt(point), point.z);
        }

        /// <summary>
        /// Calculates the height of the fluid at a given location. Override this method in order to customize the wave function.
        /// </summary>
        public virtual float GetHeightAt(Vector3 point)
        {
            return surfaceHeight;
        }
    }
}