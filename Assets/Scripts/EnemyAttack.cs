using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Movement")]
    public float speed = 3f;

    [Header("Attack Settings")]
    public float attackRange = 0.5f;      // distance to start attack
    public float attackCooldown = 1f;     // time between attack animations

    [Header("Trigger Damage Settings")]
    public int touchDamage = 1;           // damage to player on touch
    public LayerMask playerLayer;         // player layer for trigger detection

    [Header("Death")]
    public float deathDelay = 1.2f;
    [Header("Wall Avoidance")]
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.5f;


    private bool isDead = false;
    private bool isAttacking = false;

    private void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move toward player
            anim.SetBool("isRunning", true);
            Vector2 direction = (player.position - transform.position).normalized;
            // Wall detection
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                wallCheckDistance,
                wallLayer
            );

            if (hit.collider != null)
            {
                // Stop or slightly redirect instead of walking into wall
                rb.velocity = Vector2.zero;
                return;
            }

            rb.velocity = direction * speed;
        }
        else
        {
            // Stop moving and play attack
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero;

            if (!isAttacking)
                StartCoroutine(AttackRoutine());
        }

        // Flip sprite to face player
        transform.localScale = new Vector3(player.position.x > transform.position.x ? 1 : -1, 1, 1);
    }

    // Plays attack animation with cooldown
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Called by Player melee
    public void Hit()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        StopAllCoroutines(); // stop attacking

        Destroy(gameObject, deathDelay);
    }

    // Optional: visualize attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
