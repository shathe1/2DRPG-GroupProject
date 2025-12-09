using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [Header("Light Timings")]
    public float greenLightDuration = 3f;
    public float redLightDuration = 3f;

    [Header("References")]
    public SpriteRenderer floorRenderer;     // Drag the pink platform sprite here
    public Rigidbody2D playerRb;             // Drag Player → Rigidbody2D
    public PlayerMovement playerMovement;    // Drag Player → PlayerMovement script
    public Text statusText;                  // Drag UI Text ("GO / STOP")

    private bool isRedLight = false;
    private float timer = 0f;

    private void Start()
    {
        SetGreenLight();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (isRedLight)
                SetGreenLight();
            else
                SetRedLight();
        }

        // Lose condition: Player presses movement keys during RED light

        // LightController
        if (isRedLight)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                playerMovement.Die();
                enabled = false; // stop checking
            }
        }
    }

    private void SetGreenLight()
    {
        isRedLight = false;
        timer = greenLightDuration;

        floorRenderer.color = Color.green;
        statusText.text = "GO";

        playerMovement.canMove = true;  // allow movement

    }

    private void SetRedLight()
    {
        isRedLight = true;
        timer = redLightDuration;

        floorRenderer.color = Color.red;
        statusText.text = "STOP";

        playerRb.velocity = Vector2.zero; // fully stop movement
    }
}
