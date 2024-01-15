using UnityEngine;

public class OTSMovement : CharacterMovement
{
    [Header("Camera")]
    public Transform spectator;

    private float _turnSmoothTime = 0.1f;
    private float _smoothTurnVelocity;

    protected override void Move(float deltaTime)
    {
        Vector3 moveDirection;
        float angle;

        if (!_isShift)
        {
            var targetAngle = Mathf.Atan2(_inputAxis.x, _inputAxis.z) * Mathf.Rad2Deg + spectator.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _smoothTurnVelocity, _turnSmoothTime);
        }
        else
        {
            moveDirection = transform.TransformDirection(_inputAxis);
            angle = spectator.eulerAngles.y;
        }

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        _controller.Move(moveDirection.normalized * _inputAxis.magnitude * moveSpeed * deltaTime);
    }
}
