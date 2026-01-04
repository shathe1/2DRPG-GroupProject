using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private bool triggered = false;
    private DoorController doorController;

    private void Awake()
    {
        doorController = GetComponent<DoorController>();

        if (doorController == null)
        {
            Debug.LogError("ExitDoor: No DoorController found on the door!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // OPEN THE DOOR 🔑
        doorController.OpenDoor();

        // OPTIONAL: delay before scene change
        Invoke(nameof(LoadNextLevel), 1f);
    }

    private void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
