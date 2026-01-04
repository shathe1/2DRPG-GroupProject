using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Level2WinController : MonoBehaviour
{
    [Header("References")]
    public TileManager tileManager;
    public Tilemap floorTilemap;
    public Transform player;
    public DoorController door;

    [Header("Settings")]
    public float doorOpenDelay = 1.5f;

    private bool hasWon = false;

    void Update()
    {
        if (hasWon) return;

        // Get player collider bounds
        Collider2D col = player.GetComponent<Collider2D>();
        if (col == null) return;

        // Check cell under player's feet
        Vector3 feetWorldPos = new Vector3(
            col.bounds.center.x,
            col.bounds.min.y - 0.05f,
            0f
        );

        Vector3Int playerCell = floorTilemap.WorldToCell(feetWorldPos);

        if (tileManager.IsExitCell(playerCell))
        {
            Win();
        }
    }


    void Win()
    {
        if (hasWon) return;
        hasWon = true;

        // Stop player movement
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.canMove = false;

        StartCoroutine(WinSequence());
    }

    IEnumerator WinSequence()
    {
        // 1️⃣ Open door
        if (door != null)
            door.OpenDoor();

        // 2️⃣ Wait for door animation
        yield return new WaitForSeconds(doorOpenDelay);

        // 3️⃣ Hide player
        player.gameObject.SetActive(false);

        // 4️⃣ Load next level
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}
