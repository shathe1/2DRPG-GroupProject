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

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt("CurrentLevelIndex", currentIndex);
        PlayerPrefs.SetInt("NextLevelIndex", currentIndex + 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("WinScreen");
    }
}
