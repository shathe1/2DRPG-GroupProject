using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 30f;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;

            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("NextLevelIndex", currentIndex + 1);
            PlayerPrefs.SetInt("CurrentLevelIndex", currentIndex);

            SceneManager.LoadScene("WinScreen");
        }
    }
}
