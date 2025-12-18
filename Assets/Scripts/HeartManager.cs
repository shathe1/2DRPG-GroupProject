using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance;

    public int maxLives = 4;
    public int currentLives;

    public GameObject heartPrefab;
    public Transform heartsParent;
    public AudioClip loseSound;

    private GameObject[] hearts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (currentLives == 0) currentLives = maxLives;
        SetupHearts();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (heartsParent == null)
        {
            heartsParent = GameObject.Find("HeartsParent")?.transform;
        }

        SetupHearts();
    }

    public void SetupHearts()
    {
        if (heartsParent == null)
        {
            Debug.LogWarning("HeartsParent not assigned in the scene!");
            return;
        }

        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }

        hearts = new GameObject[maxLives];

        for (int i = 0; i < maxLives; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartsParent);
            hearts[i].SetActive(i < currentLives);
        }
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;

        if (hearts != null && currentLives >= 0 && currentLives < hearts.Length)
        {
            if (hearts[currentLives] != null)
            {
                hearts[currentLives].SetActive(false);
            }
        }

        if (currentLives <= 0)
        {
            AudioManager.Instance.PlaySFX(loseSound);
            SceneManager.LoadScene("LoseScreen");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetAllLives()
    {
        currentLives = maxLives;
        SetupHearts();
    }
}
