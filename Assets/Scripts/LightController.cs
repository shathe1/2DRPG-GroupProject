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

        // ❗ Lose condition: Player moves during RED light
        if (isRedLight && Mathf.Abs(playerRb.velocity.x) > 0.1f)
        {
            Debug.Log("❌ Player moved during RED light → LOSE");
            // TODO: Load lose screen or reset
        }
    }

    private void SetGreenLight()
    {
        isRedLight = false;
        timer = greenLightDuration;

        floorRenderer.color = Color.green;
        statusText.text = "GO";

        playerMovement.enabled = true;  // allow moving
    }

    private void SetRedLight()
    {
        isRedLight = true;
        timer = redLightDuration;

        floorRenderer.color = Color.red;
        statusText.text = "STOP";

        playerMovement.enabled = false; // freeze movement script
    }
}
