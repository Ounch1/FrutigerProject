using UnityEngine;

namespace BuoyancyToolkit.Examples
{
    public class CustomFluidVolume : FluidVolume
    {
        public override float GetHeightAt(Vector3 point)
        {
            // Custom wave function for buoyancy simulation
            return surfaceHeight + Mathf.Sin(point.x * 0.5f + Time.time) * 0.8f + Mathf.Sin(point.z * 0.5f + Time.time) * 0.2f;
        }
    }
}