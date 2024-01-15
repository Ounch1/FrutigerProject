using UnityEditor.UIElements;
using UnityEngine;

public class OTSCamera : TargetCamera
{
    [Header("Follow")]
    public float speed = 200f;
    public bool clampWithWalls = true;
    public LayerMask obstacles;

    [Header("Controls")]
    public float sensitivityX = 500f;
    public float sensitivityY = 500f;
    public float zoomSpeed = 150f;

    [Header("Bounds")]
    public float minAngle = -160f;
    public float maxAngle = -10f;
    public float maxDistance = 10f;
    public float minDistance = 0.35f;

    private Vector2 _sphereRadians = new Vector2(0f, 3f);
    private float _zoom;
    private Vector3 _targetPos;

    private Vector3 localPos => getLocalPos();

    private void Update()
    {
        UpdatePosition(Time.deltaTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _zoom = minDistance;
    }

    private void UpdatePosition(float deltaTime)
    {
        if (Input.GetMouseButton(1) || ServiceLocator.GetService<PlayerInput>().isShift)
        {
            _sphereRadians += new Vector2(Input.GetAxis("Mouse X") * -sensitivityX, Input.GetAxis("Mouse Y") * -sensitivityY) * deltaTime * Mathf.Deg2Rad; // Calculate camera movement
        }

        _zoom += -Input.GetAxis("Mouse ScrollWheel") * deltaTime * zoomSpeed;

        ClampValues();

        if (Mathf.Approximately(_zoom, minDistance / maxDistance))
        {
            ServiceLocator.GetService<ViewSwitcher>().isOTS = false;
        }

        _targetPos = target.position + localPos;
        transform.LookAt(target.TransformPoint(offset));

        if (clampWithWalls)
        {
            ClampWithWalls();
        }

        transform.position = Vector3.Lerp(transform.position, _targetPos, speed * deltaTime);
    }

    private void ClampValues()
    {
        _sphereRadians.y = Mathf.Clamp(_sphereRadians.y, minAngle * Mathf.Deg2Rad, maxAngle * Mathf.Deg2Rad);
        _zoom = Mathf.Clamp(_zoom, minDistance / maxDistance, 1);
    }

    private void ClampWithWalls()
    {
        var orig = target.TransformPoint(offset);
        if (Physics.SphereCast(orig, 0.3f, _targetPos - orig, out var hit, _zoom * maxDistance, obstacles))
        {
            _targetPos = hit.point;
            _targetPos += transform.forward * 0.5f;
        }
    }

    private Vector3 getLocalPos()
    {
        var x = Mathf.Cos(_sphereRadians.x) * Mathf.Sin(_sphereRadians.y);
        var y = Mathf.Cos(_sphereRadians.y);
        var z = Mathf.Sin(_sphereRadians.x) * Mathf.Sin(_sphereRadians.y);

        var pos = new Vector3(x, y, z);
        pos *= _zoom * maxDistance;
        pos += offset;

        return pos;
    }
}
