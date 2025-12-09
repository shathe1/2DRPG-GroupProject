using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Died!");

            // Get the PlayerMovement component
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.Die(); // Call the Die method
            }
        }
    }
}