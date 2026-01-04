using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance;

    [Header("Lives")]
    public int maxLives = 5;
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

        TryFindHeartsParent();
        CreateHearts();
        UpdateHeartsUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we are NOT in a gameplay scene, destroy this manager
        if (scene.name == "WinScreen" || scene.name == "LoseScreen")
        {
            Destroy(gameObject);
            return;
        }

        TryFindHeartsParent();
        CreateHearts();
        UpdateHeartsUI();

        isRespawning = false;
    }

    private void TryFindHeartsParent()
    {
        if (heartsParent == null)
            heartsParent = GameObject.Find("HeartsParent")?.transform;

        GameObject parentObj = GameObject.Find("HeartsParent");
        heartsParent = parentObj != null ? parentObj.transform : null;
    }

    private void CreateHearts()
    {
        if (heartsParent == null || heartPrefab == null)
            return;

        // Clear old hearts safely
        for (int i = heartsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(heartsParent.GetChild(i).gameObject);
        }

        hearts = new GameObject[maxLives];

        for (int i = 0; i < maxLives; i++)
        {
            hearts[i] = Instantiate(heartPrefab, heartsParent);
        }
    }

    private void UpdateHeartsUI()
    {
        if (hearts == null)
            return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < currentLives);
        }
    }

    public void LoseLife()
    {
        if (isRespawning || currentLives <= 0)
            return;

        isRespawning = true;
        currentLives--;

        UpdateHeartsUI();

        if (currentLives <= 0)
        {
            if (AudioManager.Instance != null && loseSound != null)
                AudioManager.Instance.PlaySFX(loseSound);

            Destroy(gameObject); // 🔥 CRITICAL FIX
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Instance == this)
            Instance = null;
    }
}
