using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public float meleeRange = 0.5f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {
            StartCoroutine(MeleeRoutine());
        }
    }

    IEnumerator MeleeRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Melee");

        // wait for animation to finish
        yield return new WaitForSeconds(0.5f); // match melee animation length

        isAttacking = false;
    }

    // CALLED BY ANIMATION EVENT
    public void DealMeleeDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            meleeRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>()?.Hit();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}
