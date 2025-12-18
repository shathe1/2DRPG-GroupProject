using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;

    void Start()
    {
        AudioManager.Instance.PlayMusic(backgroundMusic);
    }
}
