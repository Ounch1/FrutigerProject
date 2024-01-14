using UnityEngine;

public class FPVCamera : TargetCamera
{
    [Header("Controls")]
    public float sensitivityY = 200f;
    public float sensitivityX = 200f;
    [Header("Bounds")]
    public float minAngle = -60f;
    public float maxAngle = 60f;

    private Vector2 _sphereDegrees;

    private void Update()
    {
        UpdatePosition(Time.deltaTime);
        if (Input.GetAxis("Mouse ScrollWheel") < -Mathf.Epsilon)
        {
            ViewSwitcher.instance.isTPC = true;
        }       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _sphereDegrees.x = target.eulerAngles.y;
    }

    public void UpdatePosition(float deltaTime)
    {
        CalculateRotation(deltaTime);
        ClampRotation();

        transform.position = target.TransformPoint(offset);
        target.eulerAngles = Vector3.up * _sphereDegrees.x;
        transform.eulerAngles = target.eulerAngles;
        transform.Rotate(target.right, _sphereDegrees.y, Space.World);
    }

    private void CalculateRotation(float deltaTime)
    {
        _sphereDegrees += new Vector2(Input.GetAxis("Mouse X") * sensitivityX, - Input.GetAxis("Mouse Y") * sensitivityY) * deltaTime;
    }

    private void ClampRotation()
    {
        _sphereDegrees.y = Mathf.Clamp(_sphereDegrees.y, minAngle, maxAngle);
    }
}
