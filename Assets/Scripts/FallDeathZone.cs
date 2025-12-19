using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🔒 Do NOT run in Lose / Win screens
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "LoseScreen" || sceneName == "WinScreen")
            return;

        if (!collision.CompareTag("Player"))
            return;

        PlayerMovement pm = collision.GetComponent<PlayerMovement>();

        if (pm == null || !pm.enabled)
            return;

        if (HeartManager.Instance != null &&
            HeartManager.Instance.currentLives <= 0)
            return;

        pm.Die();
    }
}
