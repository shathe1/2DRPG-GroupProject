using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 7;
    public int currentHealth;
    public Image healthBarImage;
    public Sprite[] healthBarFrames;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            TakeDamage(1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            TakeDamage(1);
    }

    public void TakeDamage(int amount = 1)
    {
        if (!canTakeDamage || player.isDying) return;

        canTakeDamage = false;
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth > 0)
            player.StartCoroutine(player.Flicker(invincibilityTime, 0.1f));

        if (currentHealth <= 0)
            StartCoroutine(HandleDeath());

        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(invincibilityTime);
        canTakeDamage = true;
    }

    private IEnumerator HandleDeath()
    {
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        player.Die();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LoseScreen");
    }
}
