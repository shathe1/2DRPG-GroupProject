using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volume at game start
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Apply the loaded volumes to the audio sources
        if (musicSource != null)
            musicSource.volume = musicVolume;
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    void OnSceneUnloaded(Scene scene)
    {
        StopMusic();
    }



    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        if (musicSource != null)
            musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        if (sfxSource != null)
            sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // Slider callbacks
    public void OnMusicSliderChanged(float value)
    {
        SetMusicVolume(value);
    }

    public void OnSFXSliderChanged(float value)
    {
        SetSFXVolume(value);
    }
}
