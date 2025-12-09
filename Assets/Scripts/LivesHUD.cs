using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [Header("Hearts")]
    public RawImage[] hearts; // drag your pre-placed hearts here

    private int currentLives;

    private void Start()
    {
        currentLives = hearts.Length;
        UpdateHearts();
    }

    // Call this method when player loses a life
    public void LoseLife()
    {
        currentLives--;
        if (currentLives < 0) currentLives = 0;
        UpdateHearts();
    }

    // Call this method to reset lives (optional)
    public void ResetLives()
    {
        currentLives = hearts.Length;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].enabled = i < currentLives; // show or hide heart
        }
    }
}
