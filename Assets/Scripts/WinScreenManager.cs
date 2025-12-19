using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int nextIndex = PlayerPrefs.GetInt("NextLevelIndex", -1);

        if (nextIndex >= 0 && nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("Main Menu");
    }

    public void RestartLevel()
    {
        int currentIndex = PlayerPrefs.GetInt("CurrentLevelIndex", -1);
        if (currentIndex >= 0)
            SceneManager.LoadScene(currentIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
