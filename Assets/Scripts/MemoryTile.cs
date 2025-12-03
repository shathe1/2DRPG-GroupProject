using UnityEngine;
using System.Collections;

public class MemoryTile : MonoBehaviour
{
    public bool isCracked = false;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void RevealShake(float duration)
    {
        StartCoroutine(Shake(duration));
    }

    IEnumerator Shake(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isCracked)
        {
            Debug.Log("PLAYER DIED — cracked tile");
            // Insert your death logic here
        }
    }
}
