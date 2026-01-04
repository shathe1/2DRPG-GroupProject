using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false;

    private const string OPEN_PARAM = "Open";

    [Header("Sound")]
    public AudioClip doorOpenSound;

    [Header("Level 3 Logic")]
    public bool useGunRequirement = false; // enable ONLY in Level 3

    private TimerController timer;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("DoorController: No Animator found on the door!");
        }

        if (useGunRequirement)
        {
            timer = FindObjectOfType<TimerController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            Debug.Log("DOOR TRIGGERED");
            // Level 3 behavior
            if (useGunRequirement)
            {
                if (timer != null)
                {
                    timer.TryWinLevel(this);
                }
                return;
            }

            OpenDoor();
    }

    public void OpenDoor()
    {
        if (isOpened) return;

        isOpened = true;
        animator.SetBool(OPEN_PARAM, true);

        if (doorOpenSound != null)
            AudioManager.Instance.PlaySFX(doorOpenSound);
    }

    public bool IsOpened()
    {
        return isOpened;
    }
}
