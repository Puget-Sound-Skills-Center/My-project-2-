using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float deceleration = 35f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteCounter;

    [Header("Wall Slide")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallCheckDistance = 0.3f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody2D body;
    private bool isGrounded;
    private bool isWallSliding;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForceX = 8f;
    [SerializeField] private float wallJumpForceY = 10f;
    [SerializeField] private float wallJumpLockTime = 0.2f;

    private bool isWallJumping;
    private float wallJumpLockCounter;
    private int wallDirection;
    bool dead;
    Vector2 startPosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Horizontal movement
        if (!isWallJumping)
        {
            float targetSpeed = horizontalInput * speed;

            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

            float newSpeed = Mathf.MoveTowards(
                body.linearVelocity.x,
                targetSpeed,
                accelRate * Time.deltaTime
            );

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);
        }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Jump only if grounded
        if (Input.GetKeyDown(KeyCode.Space) && coyoteCounter > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            coyoteCounter = 0f;
        }

        HandleWallSlide(horizontalInput);

        // Wall Jump
        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            isWallJumping = true;
            wallJumpLockCounter = wallJumpLockTime;

            body.linearVelocity = new Vector2(
                -wallDirection * wallJumpForceX,
                wallJumpForceY
            );
        }

        if (isWallJumping)
        {
            wallJumpLockCounter -= Time.deltaTime;

            if (wallJumpLockCounter <= 0)
            {
                isWallJumping = false;
            }
        }

        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    private void HandleWallSlide(float horizontalInput)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right * Mathf.Sign(horizontalInput),
            0.6f,
            wallLayer
        );

        if (hit && !IsGrounded() && body.linearVelocity.y < 0)
        {
            isWallSliding = true;
            wallDirection = (int)Mathf.Sign(horizontalInput);

            body.linearVelocity = new Vector2(
                body.linearVelocity.x,
                -wallSlideSpeed
            );
        }
        else
        {
            isWallSliding = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckRadius,
            groundLayer
        );
    }

    public void Die()
    {
        dead = true;
    }

    public void Respawn()
    {

        dead = false;

    }
}