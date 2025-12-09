using System.Collections;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public float pullForce = 5f;
    private bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;
        // Optional: play shake animation
        if (active) StartCoroutine(ShakeMagnet());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * pullForce);
        }
    }

    private IEnumerator ShakeMagnet()
    {
        Vector3 originalPos = transform.position;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.1f;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
