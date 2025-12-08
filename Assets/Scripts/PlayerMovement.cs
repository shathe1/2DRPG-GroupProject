using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // -------------------------
        //  LEFT / RIGHT MOVEMENT
        // -------------------------
        float move = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Flip sprite
        if (move > 0)
            spriteRenderer.flipX = false;
        else if (move < 0)
            spriteRenderer.flipX = true;

        // -------------------------
        //  SET ANIMATIONS
        // -------------------------
        if (move != 0)
            animator.Play("WalkRight");   // Your walking animation
        else
            animator.Play("Idle");        // Your idle animation

        // -------------------------
        //  JUMPING
        // -------------------------
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    // Detect touching ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
