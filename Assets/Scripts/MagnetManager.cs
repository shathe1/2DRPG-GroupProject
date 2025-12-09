using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    public MagnetController[] magnets; // assign all magnets here
    public float switchInterval = 3f;  // time before switching magnet
    private int activeIndex = -1;
    private float timer;

    void Start()
    {
        timer = switchInterval;
        ActivateRandomMagnet();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ActivateRandomMagnet();
            timer = switchInterval;
        }
    }

    void ActivateRandomMagnet()
    {
        // Deactivate previous
        if (activeIndex != -1)
            magnets[activeIndex].SetActive(false);

        // Pick new random
        activeIndex = Random.Range(0, magnets.Length);
        magnets[activeIndex].SetActive(true);
    }
}
