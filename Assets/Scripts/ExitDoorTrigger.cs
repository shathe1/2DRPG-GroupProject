using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    public string winSceneName = "WinScreen";
    private DoorController door;

    private void Awake()
    {
        door = GetComponent<DoorController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached exit door!");

            door.OpenDoor();  // play animation

            // wait before scene load
            StartCoroutine(LoadWinSceneAfterDelay());
        }
    }

    private System.Collections.IEnumerator LoadWinSceneAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Wait 1 second
        SceneManager.LoadScene(winSceneName);
    }
}
