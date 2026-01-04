using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 120f;
    public TextMeshProUGUI timerText;

    public int requiredGunCount = 21;

    private bool levelEnded = false;

    void Update()
    {
        if (levelEnded)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerUI();
            LoseLevel();
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString();
    }

    void LoseLevel()
    {
        levelEnded = true;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("NextLevelIndex", currentIndex + 1);
        PlayerPrefs.SetInt("CurrentLevelIndex", currentIndex);

        SceneManager.LoadScene("LoseScreen");
    }
    public void TryWinLevel(DoorController door)
    {
        if (levelEnded) return;
        if (timeRemaining <= 0) return;
        if (GunUI.instance == null) return;

        if (GunUI.instance.GetGunCount() < requiredGunCount)
        {
            Debug.Log("Not enough guns collected!");
            return;
        }

        levelEnded = true;

        door.OpenDoor();
        StartCoroutine(WinAfterDelay());
    }


    private System.Collections.IEnumerator WinAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentIndex + 1);
    }

    void WinLevel()
    {
        levelEnded = true;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
