public class FPVMovement : CharacterMovement
{
    protected override void Move(float deltaTime)
    {
        _controller.Move(transform.TransformDirection(_inputAxis) * moveSpeed * deltaTime);
    }
}