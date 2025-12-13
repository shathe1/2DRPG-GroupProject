using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    public bool useGroundCheck = true; // false for Level 3


    private float moveInput;
    private bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public Tilemap floorTilemap;
    public float tileCheckOffsetY = -0.1f;

    private bool isDying = false;

    [Header("Tile Logic")]
    public TileManager tileManager;

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
        // -------- Tile Death Check --------
        if (!isDying && floorTilemap != null)
        {
            CheckCrackedTile();
        }

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // Check if grounded
        if (useGroundCheck && groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    


    public HeartManager heartManager;

    public void Die()
    {
        if (isDying) return;
        isDying = true;

        rb.velocity = Vector2.zero;
        rb.simulated = false;
        canMove = false;

        anim.SetTrigger("Die");

        // Let animation play for 0.4s then remove 1 heart
        Invoke(nameof(AfterDeath), 0.4f);
    }

    private void AfterDeath()
    {
        HeartManager.Instance.LoseLife();
    }



    private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }

    void CheckCrackedTile()
    {
        Vector3 worldPos = transform.position + new Vector3(0, tileCheckOffsetY, 0);
        Vector3Int cellPos = floorTilemap.WorldToCell(worldPos);

        if (tileManager == null) return;

        if (tileManager.IsCrackedAt(cellPos))
        {
            Die();
        }
    }


}
