using UnityEngine;

namespace BuoyancyToolkit
{
    public enum BuoyancyQuality : int
    {
        Low, Medium, High, Custom
    }

    public interface IBuoyancyForce
    {
        /// <summary>
        /// The collider that will be used to calculate the buoyancy properties of the rigidbody. The collider should be convex for stability reasons.
        /// </summary>
        Collider BuoyancyCollider
        {
            get;
            set;
        }
        
        /// <summary>
        /// The quality of the simulation. High values trade performance for simulation quality and vice versa.
        /// </summary>
        BuoyancyQuality Quality
        {
            get;
            set;
        }
        
        /// <summary>
        /// The number of sample points used per axis for the buoyancy simulation. High values trade performance for simulation quality and vice versa.
        /// </summary>
        int Samples
        {
            get;
            set;
        }
        
        /// <summary>
        /// A toggle indicating whether this BuoyancyForce uses weighting. Weighting enables easy tweaking of the buoyancy behaviour. If weighting is not used, realistic proportions, rigidbody masses and fluid densities are required for realistic behaviour.
        /// </summary>
        bool UseWeighting
        {
            get;
            set;
        }
        
        /// <summary>
        /// A value indicating the strength of the buoyancy force when weighting is enabled. A weight factor of 1 results in enough force to counteract gravity and the rigidbody will stay in equilibrium within the fluid. A weight factor of 2 results in a net force equal to gravity but in the opposite direction (making the rigidbody float in the fluid) and so on.
        /// </summary>
        float WeightFactor
        {
            get;
            set;
        }
        
        /// <summary>
        /// A scalar that is multiplied by the fluid volume's drag value before being set as linear drag to the submerged rigidbody.
        /// </summary>
        float DragScalar
        {
            get;
            set;
        }
        
        /// <summary>
        /// A scalar that is multiplied by the fluid volume's angular drag value before being set as angular drag to the submerged rigidbody.
        /// </summary>
        float AngularDragScalar
        {
            get;
            set;
        }
        
        /// <summary>
        /// The base linear drag that should be applied to the rigidbody. Set to the drag value of the connected rigidbody at the start of the scene.
        /// </summary>
        float NonfluidDrag
        {
            get;
            set;
        }
        
        /// <summary>
        /// The base angular drag that should be applied to the rigidbody. Set to the angularDrag value of the connected rigidbody at the start of the scene.
        /// </summary>
        float NonfluidAngularDrag
        {
            get;
            set;
        }

        /// <summary>
        /// A bitmask indicating which FluidVolume layers should be ignored by this BuoyancyForce.
        /// </summary>
        LayerMask IgnoreLayers
        {
            get;
            set;
        }

        /// <summary>
        /// The fluid volume that currently affects this buoyancy force.
        /// </summary>
        FluidVolume FluidVolume
        {
            get;
        }
        
        /// <summary>
        /// A value indicating whether the buoyancy collider is submerged in a fluid volume.
        /// </summary>
        bool IsSubmerged
        {
            get;
        }
        
        /// <summary>
        /// A value indicating whether the buoyancy collider is completely submerged in a fluid volume.
        /// </summary>
        bool IsCompletelySubmerged
        {
            get;
        }
        
        /// <summary>
        /// The approximate volume of the buoyancy collider.
        /// </summary>
        float Volume
        {
            get;
        }
        
        /// <summary>
        /// The approximate submerged volume of the buoyancy collider.
        /// </summary>
        float SubmergedVolume
        {
            get;
        }

        /// <summary>
        /// A toggle indicating whether or not debug visualizations are rendered.
        /// </summary>
        bool DebugVisualization
        {
            get;
            set;
        }
    }
}