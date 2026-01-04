using UnityEngine;

public class GunCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GunUI.instance.AddGun();
            Destroy(gameObject);
        }
    }
}
