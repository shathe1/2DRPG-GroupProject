using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Control")]
    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector3 baseScale; // store original prefab scale so flipping preserves size

    [Header("Optional Float Controls (Level 3)")]
    public bool allowFreeVerticalMovement = false; // leave false for other levels
    public float verticalMoveSpeed = 5f;


    private float moveInput;
    private bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseScale = transform.localScale; // remember original scale
    }

    private void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // Horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip sprite left/right while preserving original scale magnitude
        if (moveInput != 0)
        {
            float sign = moveInput > 0 ? 1f : -1f;
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x) * sign, baseScale.y, baseScale.z);
        }

        // Update walk animation
        anim.SetFloat("SpeedX", moveInput); // feed blend tree: -1 left, 0 idle, +1 right

        // Vertical movement (level 3)
        if (allowFreeVerticalMovement)
        {
            float verticalInput = Input.GetAxisRaw("Vertical"); // Up/Down or W/S
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * verticalMoveSpeed);
        }

        // Jump input (space or up arrow)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("IsJumping", !isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Die()
    {
        canMove = false;                           // stop player movement
        rb.velocity = Vector2.zero;                // freeze movement immediately
        anim.SetTrigger("Die");                    // play die animation
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
