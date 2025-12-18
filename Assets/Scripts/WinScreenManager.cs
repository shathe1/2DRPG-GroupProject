using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public GameObject winScreenCanvas;
    public AudioClip winSound;

    public void ShowWinScreen()
    {
        AudioManager.Instance.PlaySFX(winSound);
        winScreenCanvas.SetActive(true);
        Time.timeScale = 0f; // freeze gameplay
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // resume gameplay

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // If we are at the last level, go to main menu instead
        if (currentSceneIndex + 1 < totalScenes)
        {
            SceneManager.LoadScene(currentSceneIndex + 1); // load next level
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); // fallback if no next level exists
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
