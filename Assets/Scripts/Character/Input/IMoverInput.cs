public interface IMoverInput : IInput
{
    public void GetMovementAxis(out float vertical, out float horizontal, out bool isShift);
}