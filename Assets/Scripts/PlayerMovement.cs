using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public bool isMoving = false;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Move right/left
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Animation: running or idle
        isMoving = move != 0;
        anim.SetBool("isMoving", isMoving);

        // Flip player depending on direction
        if (move > 0) transform.localScale = new Vector3(1, 1, 1);
        if (move < 0) transform.localScale = new Vector3(-1, 1, 1);

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}
