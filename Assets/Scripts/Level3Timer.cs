using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timerText;

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
                Debug.Log("Level Completed!");
            }
        }
    }
}
