using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public float meleeRange = 1f;
    public int meleeDamage = 1;
    public LayerMask enemyLayer; // make sure enemies are on this layer

    void Update()
    {
        // Trigger melee attack
        if (Input.GetKeyDown(KeyCode.Space)) // change to your melee button
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        anim.SetTrigger("Melee"); // play animation

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>()?.Hit();
        }
    }

    // Optional: visualize melee range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
