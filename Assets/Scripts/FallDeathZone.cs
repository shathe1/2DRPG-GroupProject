using UnityEngine;

public class FallDeathZone : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.Die();
            }
        }
    }
}
