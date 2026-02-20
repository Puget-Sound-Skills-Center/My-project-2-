using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 12f;

    public float wallSlideSpeed = 2f;
    public LayerMask groundLayer;
    public Transform wallCheck;

    private Rigidbody2D rb;
    private float horizontal;
    private bool isGrounded;
    private bool isWalled;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Sprinting Logic
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            currentSpeed *= sprintMultiplier;
        }

        // Apply Horizontal Movement
        rb.linearVelocity = new Vector2(horizontal * currentSpeed, rb.linearVelocity.y);

        WallSlide();
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    private bool IsGrounded()
    {
        // Checks if a small circle at the player's feet hits the ground layer
        return Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.5f, 0), 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        // Checks if the wallCheck object is touching a wall
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }
}