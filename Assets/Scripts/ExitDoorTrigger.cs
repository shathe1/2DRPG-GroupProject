using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (HeartManager.Instance != null &&
            HeartManager.Instance.currentLives > 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
