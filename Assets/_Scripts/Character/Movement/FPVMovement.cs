using UnityEngine;

public class FPVMovement : CharacterMovement
{
    protected override void Awake()
    {
        base.Awake();
        // No need to get a separate Rigidbody, it's already in the base class.
    }

    protected override void Move(float deltaTime)
    {
        rb.MovePosition(rb.position + transform.TransformDirection(_inputAxis) * moveSpeed * deltaTime);
    }
}
