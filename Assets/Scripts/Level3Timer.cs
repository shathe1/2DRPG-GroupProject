using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;

    // Name of the next scene to load when the level is completed
    public string nextLevelSceneName = "Main Menu"; 

    void Update()
    {
        if (timerText != null)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerText.text = "Time: 0";
                CompleteLevel();
            }
        }
    }

    void CompleteLevel()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
