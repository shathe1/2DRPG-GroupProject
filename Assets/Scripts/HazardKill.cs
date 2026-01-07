using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
                Time.timeScale = 1f;
                SceneManager.LoadScene("LoseScreen");
            }
        }
    }
    
}
