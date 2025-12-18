using UnityEngine;

public class PlayerTriggeredPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private bool playerOnPlatform = false;
    private Rigidbody2D rb;
    private Transform targetPoint;

    private Vector3 lastPlatformPos;
    private Transform playerTransform;
    private bool movingToB = false;
    private bool movingToA = false;

    private const float stopDistance = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        targetPoint = pointB;
        lastPlatformPos = transform.position;
    }

    void FixedUpdate()
    {
        lastPlatformPos = transform.position;

        // --- Move toward B ---
        if (movingToB)
        {
            MovePlatform(pointB);

            if (Vector2.Distance(transform.position, pointB.position) < stopDistance)
            {
                transform.position = pointB.position;  // Snap exactly
                movingToB = false;
            }
        }

        // --- Move toward A ---
        if (movingToA)
        {
            MovePlatform(pointA);

            if (Vector2.Distance(transform.position, pointA.position) < stopDistance)
            {
                transform.position = pointA.position;  // Snap exactly
                movingToA = false;
            }
        }
    }


    void MovePlatform(Transform target)
    {
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        // Move player with platform
        if (playerOnPlatform && playerTransform != null)
        {
            Vector3 delta = transform.position - lastPlatformPos;
            playerTransform.position += delta;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
            playerTransform = collision.collider.transform;

            movingToA = false;   // stop going down
            movingToB = true;    // start going up
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
            playerTransform = null;

            movingToB = false;   // stop going up
            movingToA = true;    // go back down
        }
    }
}
