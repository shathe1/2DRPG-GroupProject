using UnityEngine;
using System.Collections;

public class HowToPlayPlayerDemo : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Start demo loop
        StartCoroutine(DemoLoop());
    }

    IEnumerator DemoLoop()
    {
        while (true)
        {
            // ▶ RUN
            animator.SetFloat("SpeedX", 1f);
            animator.SetBool("IsJumping", false);
            yield return new WaitForSeconds(2f);

            // ⏸ STOP
            animator.SetFloat("SpeedX", 0f);
            yield return new WaitForSeconds(0.5f);

            // ⬆ JUMP
            animator.SetBool("IsJumping", true);
            yield return new WaitForSeconds(1f);

            // ⬇ LAND
            animator.SetBool("IsJumping", false);
            yield return new WaitForSeconds(0.5f);

            // 🧎 SLIDE (if you use SpeedX + animation blend)
            animator.SetFloat("SpeedX", 0.5f);
            yield return new WaitForSeconds(1f);

            // 🛑 IDLE
            animator.SetFloat("SpeedX", 0f);
            yield return new WaitForSeconds(1f);
        }
    }
}
