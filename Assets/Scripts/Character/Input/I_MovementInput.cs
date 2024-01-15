public interface I_MovementInput : I_Input
{
    public void GetMovementAxis(out float vertical, out float horizontal, out bool isShift);
}