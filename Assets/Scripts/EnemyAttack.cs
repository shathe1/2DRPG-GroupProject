using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float attackRange = 1f;
    public Transform attackPoint; // assign in inspector
    public float attackRadius = 0.5f; // size of the hit
    public int attackDamage = 1;      // how much damage enemy deals
    public LayerMask playerLayer;     // assign player layer
    public float deathDelay = 1.2f; // adjust to match Die animation length


    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Run toward player
            anim.SetBool("isRunning", true);
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            // Stop moving and attack
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Attack");
        }

        // Flip sprite to face player
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // This function is called by an Animation Event at the exact frame of attack
    public void DealDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<HealthManager>()?.TakeDamage(attackDamage);
        }
    }


    public void Hit()
    {
        if (isDead) return; // prevent replay
        isDead = true;

        anim.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        Destroy(gameObject, deathDelay);
    }

}
