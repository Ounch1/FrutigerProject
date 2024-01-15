using UnityEngine;

public abstract class TargetCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    protected virtual void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected virtual void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
