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

    protected Rigidbody rb;  // Reference to the Rigidbody component
    protected I_MovementInput _input;

    protected Vector3 _inputAxis;
    protected bool _isShift;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Ensure the Rigidbody doesn't rotate due to physics.

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
        _inputAxis = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);
    }

    protected virtual void UpdateMovement(float deltaTime)
    {
        applyGravity();
        applyFall();
        applyJump();
        applyMovement();

        void applyGravity()
        {
            rb.AddForce(Physics.gravity * rb.mass);
        }

        void applyFall()
        {
            if (isGrounded && rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
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
            if (!Mathf.Approximately(_inputAxis.sqrMagnitude, Mathf.Epsilon) || _isShift)
            {
                Move(deltaTime);
            }
        }
    }

    protected abstract void Move(float deltaTime);

    protected virtual void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), rb.velocity.z);
    }

    private bool CheckGround()
    {
        return Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
    }
}
