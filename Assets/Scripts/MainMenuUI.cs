using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");  
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    // ⭐ THIS FUNCTION IS MISSING IN YOUR PROJECT — ADD IT ⭐
    public void ContinueGame()
    {
        // Load the last unlocked level
        int level = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
        SceneManager.LoadScene("Level " + level);
    }
}
