using UnityEngine;

public class TPCMover : CharacterMover
{
    [Header("Camera")]
    public Transform spectator;

    private float _turnSmoothTime = 0.1f;
    private float _smoothTurnVelocity;

    protected override void Move(float deltaTime)
    {
        var targetAngle = Mathf.Atan2(_inputAxis.x, _inputAxis.z) * Mathf.Rad2Deg + spectator.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _smoothTurnVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDirection.normalized * moveSpeed * deltaTime);
    }
}
