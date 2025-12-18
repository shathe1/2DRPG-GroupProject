using UnityEngine;

public class FallDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.Die();
            }
        }
    }
}
