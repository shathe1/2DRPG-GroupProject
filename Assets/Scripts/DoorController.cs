using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool opened = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (opened) return;   // 🔒 stops looping
        opened = true;
        animator.SetBool("Open", true);
    }
}
