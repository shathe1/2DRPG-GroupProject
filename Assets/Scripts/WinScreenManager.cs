using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public AudioClip winSound;

    public void ShowWinScreen()
    {
        AudioManager.Instance.PlaySFX(winSound);
    }
    public void LoadNextLevel()
    {
        int nextIndex = PlayerPrefs.GetInt("NextLevelIndex", -1);

        if (nextIndex >= 0 && nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void RestartLevel()
    {
        int nextIndex = PlayerPrefs.GetInt("NextLevelIndex", -1);
        SceneManager.LoadScene(nextIndex - 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
