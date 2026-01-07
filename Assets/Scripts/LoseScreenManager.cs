using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
    }

    // Restart the level the player lost
    public void RestartLevel()
    {
        Time.timeScale = 1f; // 🔑 CRITICAL
        AudioManager.Instance.StopMusic();

        string lastLevel = PlayerPrefs.GetString(
            "LastLevel",
            SceneManager.GetActiveScene().name
        );

        SceneManager.LoadScene(lastLevel);
    }

    // Go back to Main Menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene("Main Menu");
    }

    // Exit the game
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
