using UnityEngine;

namespace BuoyancyToolkit.Helpers
{
    [RequireComponent(typeof(Rigidbody))]
    public class SetCenterOfMass : MonoBehaviour
    {
        public Vector3 desiredCenterOfMass;

        public void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = desiredCenterOfMass;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.TransformPoint(desiredCenterOfMass), 0.25f);
        }
    }
}