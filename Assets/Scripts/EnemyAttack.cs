using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float attackRange = 1f;

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
            // Attack player
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Attack");
        }

        // Flip to face player
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Hit()
    {
        isDead = true;
        anim.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        // Optional: destroy object after death animation
        // Destroy(gameObject, 2f);
    }
}
