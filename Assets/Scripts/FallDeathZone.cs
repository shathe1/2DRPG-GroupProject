using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only affect the player
        if (!collision.CompareTag("Player")) return;

        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if (pm == null || pm.isDying) return;

        // Trigger the fall death sequence
        pm.DieFall();
    }
}
