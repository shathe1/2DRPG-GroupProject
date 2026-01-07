using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Control")]
    public bool canMove = true;

    public Rigidbody2D rb;
    private Animator anim;

    private Vector3 baseScale;

    [Header("Optional Float Controls (Level 3)")]
    public bool allowFreeVerticalMovement = false;
    public float verticalMoveSpeed = 5f;
    public bool useGroundCheck = true;

    private float moveInput;
    private bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public Tilemap floorTilemap;
    public float tileCheckOffsetY = -0.1f;

    public bool isDying = false;

    [Header("Tile Logic")]
    public TileManager tileManager;

    [Header("Win Condition")]
    public DoorController door;

    [Header("Audio")]
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip dieSound;
    public AudioClip winSound;
    [Header("Audio Sources")]
    public AudioSource runSource;

    private bool hasWon = false;
    public SpriteRenderer spriteRenderer; // assign your player's SpriteRenderer in Inspector


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseScale = transform.localScale;
    }

    private void Update()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            float sign = moveInput > 0 ? 1f : -1f;
            transform.localScale = new Vector3(Mathf.Abs(baseScale.x) * sign, baseScale.y, baseScale.z);
        }

        anim.SetFloat("SpeedX", moveInput);

        if (allowFreeVerticalMovement)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * verticalMoveSpeed);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }

        if (floorTilemap != null && tileManager != null)
        {
            CheckCrackedTileDeath();
        }

        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            if (!runSource.isPlaying)
            {
                runSource.Play();
            }
        }
        else
        {
            if (runSource.isPlaying)
            {
                runSource.Stop();
            }
        }

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        if (useGroundCheck && groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AudioManager.Instance.PlaySFX(jumpSound);
    }

    // ===================== DEATH LOGIC =====================


    public void Die()
    {
        if (isDying) return;

        isDying = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        rb.velocity = Vector2.zero;
        rb.simulated = false;
        canMove = false;

        anim.SetTrigger("Die");
        AudioManager.Instance.PlaySFX(dieSound);

        // DO NOT reload scene here!
    }


    private void OnEnable()
    {
        // Reset isDying when scene reloads
        isDying = false;

        // Re-enable collider in case scene reused player object
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }



    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(0.4f);

        if (HeartManager.Instance != null)
        {
            HeartManager.Instance.LoseLife();
        }
    }

    // ======================================================

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void CheckCrackedTileDeath()
    {
        if (isDying) return;

        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector3 worldPos = new Vector3(bounds.center.x, bounds.min.y - 0.05f, 0);
        Vector3Int cellPos = floorTilemap.WorldToCell(worldPos);

        if (cellPos == tileManager.exitCell)
        {
            Win();
            return;
        }

        if (!tileManager.IsCrackedAt(cellPos))
            return;

        PlatformData platform = tileManager.GetPlatformAt(cellPos);
        if (platform == null)
            return;

        StartCoroutine(DieAfterPlatformEffect(platform));
    }

    IEnumerator DieAfterPlatformEffect(PlatformData platform)
    {
        canMove = false;

        yield return StartCoroutine(
            tileManager.PlayCrackedPlatformEffect(platform)
        );

        DieFall();
    }

    void Win()
    {
        if (hasWon) return;

        hasWon = true;
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.simulated = false;

        StartCoroutine(WinSequence());
    }

    IEnumerator WinSequence()
    {
        // Play door animation
        if (door != null)
            door.OpenDoor();

        // Small delay for animation & feedback
        yield return new WaitForSecondsRealtime(1.5f);

        // Play win sound
        AudioManager.Instance.PlaySFX(winSound);

        // Optional: hide player
        gameObject.SetActive(false);

        // Load next level
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // Last level completed → return to Main Menu
            SceneManager.LoadScene("Main Menu");
        }
    }

    public IEnumerator Flicker(float duration, float interval)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        // Make sure sprite is visible at the end
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }
    public void DieFall()
    {
        if (isDying) return;

        isDying = true;

        // Stop movement but let gravity act
        rb.velocity = Vector2.zero;
        canMove = false;
        rb.simulated = true; // keep physics so player falls naturally

        anim.SetTrigger("Die"); // play death animation
        AudioManager.Instance.PlaySFX(dieSound);

        // Immediately go to lose screen after animation length
        StartCoroutine(FallDeathSequence());
    }

    private IEnumerator FallDeathSequence()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name); // match your death animation length
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoseScreen");
    }


}
