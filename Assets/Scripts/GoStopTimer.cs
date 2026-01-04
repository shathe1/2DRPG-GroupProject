using UnityEngine;
using UnityEngine.UI;

public class GoStopTimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text statusText; // GO / STOP (existing)
    [SerializeField] private Text timerText;  // number beside it

    [Header("Durations (must match LightController)")]
    [SerializeField] private float greenDuration = 3f;
    [SerializeField] private float redDuration = 3f;

    private float currentTimer;
    private string lastState;

    private void Start()
    {
        if (statusText == null || timerText == null)
        {
            Debug.LogError("GoStopTimerUI: Text references missing!");
            enabled = false;
            return;
        }

        lastState = statusText.text;
        ResetTimer();
    }

    private void Update()
    {
        // detect GO ↔ STOP switch
        if (statusText.text != lastState)
        {
            lastState = statusText.text;
            ResetTimer();
        }

        currentTimer -= Time.deltaTime;
        if (currentTimer < 0f) currentTimer = 0f;

        timerText.text = Mathf.CeilToInt(currentTimer).ToString();
    }

    private void ResetTimer()
    {
        if (statusText.text == "GO")
        {
            currentTimer = greenDuration;
            timerText.color = Color.green;
        }
        else if (statusText.text == "STOP")
        {
            currentTimer = redDuration;
            timerText.color = Color.red;
        }
    }
}
