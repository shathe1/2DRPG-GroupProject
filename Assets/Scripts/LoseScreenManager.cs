using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    public void RestartLevel()
    {
        // Reload the last played level
        SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel", "Level 1"));
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
