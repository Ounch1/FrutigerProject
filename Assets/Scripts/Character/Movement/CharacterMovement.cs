using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    [Header("Metrics")]
    public float moveSpeed = 10f;
    public float jumpHeight = 3f;
    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundMask;

    public bool isGrounded => CheckGround();

    protected CharacterController _controller;
    protected I_MovementInput _input;

    protected Vector3 _inputAxis;
    protected bool _isShift;
    private Vector3 _horizontalVelocity;

    protected virtual void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<CharacterInput>();
    }

    protected virtual void Update()
    {
        UpdateInputs();
        UpdateMovement(Time.deltaTime);
    }

    protected virtual void UpdateInputs()
    {
        _input.GetMovementAxis(out var vertical, out var horizontal, out _isShift);
        _inputAxis = new Vector3(horizontal, 0f, vertical).normalized;
    }

    protected virtual void UpdateMovement(float deltaTime)
    {
        applyGravity();
        applyFall();
        applyJump();
        applyMovement();

        void applyGravity()
        {
            _horizontalVelocity += Physics.gravity * deltaTime;
            _controller.Move(_horizontalVelocity * deltaTime);
        }

        void applyFall()
        {
            if (isGrounded && _horizontalVelocity.y < 0)
            {
                _horizontalVelocity.y = -2f;
            }              
        }

        void applyJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }           
        }

        void applyMovement()
        {
            if (!Mathf.Approximately(_inputAxis.sqrMagnitude, Mathf.Epsilon))
            {
                Move(deltaTime);
            } 
        }
    }

    protected virtual void Jump()
    {
        _horizontalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    }

    private bool CheckGround()
    {
        return Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
    }

    protected abstract void Move(float deltaTime);
}