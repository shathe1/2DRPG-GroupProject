using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerUI();

            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("NextLevelIndex", currentIndex + 1);
            PlayerPrefs.SetInt("CurrentLevelIndex", currentIndex);

            SceneManager.LoadScene("WinScreen");
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString();
    }
}
