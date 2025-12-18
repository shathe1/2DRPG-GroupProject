using UnityEngine;

public class PlayerTriggeredPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private bool playerOnPlatform = false;
    private Rigidbody2D rb;
    private Transform targetPoint;

    private Vector3 lastPlatformPos;   // for delta movement of platform
    private Transform playerTransform; // reference to player
    private bool moving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        targetPoint = pointB;
        lastPlatformPos = transform.position;
    }

    void FixedUpdate()
    {
        if (!moving) 
        {
            lastPlatformPos = transform.position;
            return;
        }

        // --- Move platform ---
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            targetPoint.position,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        // --- Move player by delta movement ---
        if (playerOnPlatform && playerTransform != null)
        {
            Vector3 delta = transform.position - lastPlatformPos;
            playerTransform.position += delta;
        }

        lastPlatformPos = transform.position;

        // --- Stop at point B ---
        if (Vector2.Distance(rb.position, pointB.position) < 0.05f)
        {
            moving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
            moving = true;
            playerTransform = collision.collider.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
            playerTransform = null;
        }
    }
}
