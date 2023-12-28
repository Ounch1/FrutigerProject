using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [Space]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private Vector3 horizontalVelocity;
    private bool isGrounded;

    private float turnSmoothTime = 0.1f;
    private float smoothTurnVelocity;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);

        // Gravity
        horizontalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(horizontalVelocity * Time.deltaTime);

        // Making sure player falls correctly when walking off a platform
        if (isGrounded && horizontalVelocity.y < 0)
        {
            horizontalVelocity.y = -2f;
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            horizontalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate which way the player should be facing (No clue how this works)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move player
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
