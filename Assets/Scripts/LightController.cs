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
    public PlayerMovement playerMovement;
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
        if (isRedLight)
        {
            bool moved = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                         Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                         Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                         Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);

            if (moved)
            {
                playerMovement.Die();
                enabled = false;
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
}
