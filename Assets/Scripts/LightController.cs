using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    [Header("Light Timings")]
    public float greenLightDuration = 3f;
    public float redLightDuration = 3f;

    [Header("References")]
    public List<SpriteRenderer> floors;    // Add all floors here manually
    public Rigidbody2D playerRb;
    public HealthManager playerHealth;     // Changed from PlayerMovement to HealthManager
    public PlayerMovement playerMovement;  // Needed for canMove toggle
    public Text statusText;

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

        // Movement check during red light
        if (isRedLight && playerHealth.currentHealth > 0)
        {
            bool moved = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                         Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                         Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                         Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);

            if (moved)
            {
                // Damage player instead of instant death
                playerHealth.TakeDamage(1);

                // Optional: you can knock player back a bit
                playerRb.velocity = Vector2.zero;

                // Disable further input for a tiny moment to prevent multi-damage in one frame
                playerMovement.canMove = false;
                StartCoroutine(EnableMovementAfterDelay(0.1f));
            }
        }
    }

    private void SetGreenLight()
    {
        isRedLight = false;
        timer = greenLightDuration;

        foreach (var f in floors)
            f.color = Color.green;

        statusText.text = "GO";
        playerMovement.canMove = true;
    }

    private void SetRedLight()
    {
        isRedLight = true;
        timer = redLightDuration;

        foreach (var f in floors)
            f.color = Color.red;

        statusText.text = "STOP";
        playerRb.velocity = Vector2.zero;
    }

    private System.Collections.IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerHealth.currentHealth > 0)
            playerMovement.canMove = true;
    }
}
