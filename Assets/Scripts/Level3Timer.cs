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

            PlayerPrefs.SetString("NextLevel", "Main Menu"); // change per level
            PlayerPrefs.SetString("ReplayLevel", SceneManager.GetActiveScene().name);

            SceneManager.LoadScene("WinScreen");
        }
    }
}
