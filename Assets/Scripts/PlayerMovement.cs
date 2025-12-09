using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;


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

    public Tilemap floorTilemap;
    public float tileCheckOffsetY = -0.1f;

    private bool isDying = false;


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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("IsJumping", !isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void CheckCrackedTile()
    {
        // Slightly below player's feet
        Vector3 worldPos = transform.position + new Vector3(0, tileCheckOffsetY, 0);
        Vector3Int cellPos = floorTilemap.WorldToCell(worldPos);

        MemoryTileAsset tile = floorTilemap.GetTile<MemoryTileAsset>(cellPos);

        if (tile == null) return;
        if (!tile.isCracked) return;

        // Trigger animation BEFORE death
        StartCoroutine(HandleCrackedTileDeath(tile, cellPos));
    }

    IEnumerator HandleCrackedTileDeath(MemoryTileAsset tile, Vector3Int cellPos)
    {
        isDying = true;
        canMove = false;

        // Play step-on-cracked effect (red flash + shake)
        yield return StartCoroutine(tile.StepOnCracked(floorTilemap, cellPos));

        // THEN player dies
        Die();
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

    private MemoryTileAsset GetTileBelowPlayer()
    {
        Vector3 worldPos = transform.position;
        Vector3Int tilePos = floorTilemap.WorldToCell(worldPos);

        return floorTilemap.GetTile<MemoryTileAsset>(tilePos);
    }

}
