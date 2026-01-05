using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level4TimerWin : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 60f;   // seconds
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    private bool finished = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        if (finished) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            WinLevel();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(currentTime).ToString();
    }

    void WinLevel()
    {
        finished = true;

        // Optional: stop time
        Time.timeScale = 1f;

        // Load Main Menu
        SceneManager.LoadScene("Main Menu");
    }
}
