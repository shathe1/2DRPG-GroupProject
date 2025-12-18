using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;

    private bool isOpened = false;

    private const string OPEN_PARAM = "Open";

    [Header("Sound")]
    public AudioClip doorOpenSound;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("DoorController: No Animator found on the door!");
        }
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
