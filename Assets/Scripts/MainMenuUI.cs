using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Intro_Level1");  
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
