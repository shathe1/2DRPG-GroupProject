using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance;

    [Header("Lives")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI")]
    public GameObject heartPrefab;
    public Transform heartsParent;

    [Header("Audio")]
    public AudioClip loseSound;

    private GameObject[] hearts;
    private bool isRespawning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Initialize lives ONLY ONCE
        if (currentLives <= 0)
            currentLives = maxLives;

        CreateHearts();
        UpdateHeartsUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-link UI parent after scene reload
        heartsParent = GameObject.Find("HeartsParent")?.transform;

        CreateHearts();
        UpdateHeartsUI();

        isRespawning = false;
    }

    private void CreateHearts()
    {
        if (heartsParent == null) return;

        foreach (Transform child in heartsParent)
            Destroy(child.gameObject);

        hearts = new GameObject[maxLives];

        for (int i = 0; i < maxLives; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartsParent);
        }
    }

    private void UpdateHeartsUI()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }

    public void LoseLife()
    {
        if (isRespawning) return;
        if (currentLives <= 0) return;

        isRespawning = true;
        currentLives--;

        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            AudioManager.Instance.PlaySFX(loseSound);
            SceneManager.LoadScene("LoseScreen");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetAllLives()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
    }
}
