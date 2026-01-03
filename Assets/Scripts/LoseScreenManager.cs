using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    // Restart the level the player lost
    public void RestartLevel()
    {
        string lastLevel = PlayerPrefs.GetString("LastLevel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(lastLevel);
    }

    // Go back to Main Menu
    public void GoToMainMenu()
    {
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
