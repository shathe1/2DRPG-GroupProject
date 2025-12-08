using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public float pullForce = 5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction from player to magnet
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                rb.AddForce(direction * pullForce);
            }
        }
    }
}