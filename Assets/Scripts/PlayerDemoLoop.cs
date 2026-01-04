using UnityEngine;
using System.Collections;

public class PlayerDemoLoop : MonoBehaviour
{
    [Header("Animator Params")]
    public string runBoolName = "isRunning";
    public string jumpTriggerName = "Jump";

    [Header("Timing")]
    public float runTime = 1.5f;
    public float pauseTime = 0.3f;
    public float jumpDelay = 0.2f;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DemoRoutine());
    }

    IEnumerator DemoRoutine()
    {
        while (true)
        {
            // run
            if (anim) anim.SetBool(runBoolName, true);
            yield return new WaitForSeconds(runTime);

            // stop
            if (anim) anim.SetBool(runBoolName, false);
            yield return new WaitForSeconds(pauseTime);

            // jump
            if (anim) anim.SetTrigger(jumpTriggerName);
            yield return new WaitForSeconds(jumpDelay);

            yield return new WaitForSeconds(1f);
        }
    }
}
