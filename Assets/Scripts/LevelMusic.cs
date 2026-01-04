using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }
}
