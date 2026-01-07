using UnityEngine;

public class LoseScreenMusic : MonoBehaviour
{
    public AudioClip loseMusic;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(loseMusic);
        }
    }
}
