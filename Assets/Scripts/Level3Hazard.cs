using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    private HashSet<GameObject> alreadyDamaged = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (alreadyDamaged.Contains(collision.gameObject)) return;

        alreadyDamaged.Add(collision.gameObject);

        // Take damage instead of instant death
        collision.GetComponent<HealthManager>().TakeDamage();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            alreadyDamaged.Remove(collision.gameObject);
    }
}
