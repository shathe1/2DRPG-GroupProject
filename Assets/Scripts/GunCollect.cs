using UnityEngine;

public class GunCollect : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // 🔊 Play sound
        if (AudioManager.Instance != null && collectSound != null)
        {
            AudioManager.Instance.PlaySFX(collectSound);
        }

        // 🎯 Collect gun
        GunUI.instance.AddGun();

        // ❌ Remove collectible
        Destroy(gameObject);
    }
}
