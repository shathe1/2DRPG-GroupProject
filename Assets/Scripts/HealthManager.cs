using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 7;
    public int currentHealth;

    [Header("Health Bar UI")]
    public Image healthBarImage;
    public Sprite[] healthBarFrames; // 7 sprites for 1-7 HP

    [Header("Damage Settings")]
    public float invincibilityTime = 0.5f;
    private bool canTakeDamage = true;

    private PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarImage != null && healthBarFrames.Length == maxHealth)
        {
            int index = Mathf.Clamp(currentHealth - 1, 0, maxHealth - 1);
            healthBarImage.sprite = healthBarFrames[index];
        }
    }

    public void TakeDamage(int amount = 1)
    {
        if (!canTakeDamage || player.isDying) return;

        canTakeDamage = false;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth > 0)
        {
            // Flicker while invincible
            player.StartCoroutine(player.Flicker(invincibilityTime, 0.1f));
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }

        StartCoroutine(DamageCooldown());
    }


    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(invincibilityTime);
        canTakeDamage = true;
    }

    private IEnumerator HandleDeath()
    {
        player.Die(); // triggers animation, disables movement

        // Wait for death animation to finish
        yield return new WaitForSeconds(0.5f);

        // Show lose screen (replace with your own lose screen logic)
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
}
