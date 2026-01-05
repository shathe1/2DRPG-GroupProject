using System.Collections;
using UnityEngine;

public class HazardRandomController : MonoBehaviour
{
    [Header("Hazard Scripts (any movement script)")]
    public MonoBehaviour[] hazards;

    [Header("Timing")]
    public float minDelay = 1f;
    public float maxDelay = 3f;

    [Header("Options")]
    public bool allowSameHazardTwice = false;

    private MonoBehaviour lastHazard;

    void Start()
    {
        StartCoroutine(RandomActivationLoop());
    }

    IEnumerator RandomActivationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            if (hazards.Length == 0)
                continue;

            MonoBehaviour chosen;

            do
            {
                chosen = hazards[Random.Range(0, hazards.Length)];
            }
            while (!allowSameHazardTwice && chosen == lastHazard && hazards.Length > 1);

            lastHazard = chosen;

            // 🔥 Activate the hazard
            chosen.enabled = true;
        }
    }
}
