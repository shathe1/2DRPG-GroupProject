using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public float meleeRange = 0.5f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

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

        yield return new WaitForSeconds(0.2f); // timing to match animation swing

        DealMeleeDamage();
        isAttacking = false;
    }

    void DealMeleeDamage()
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}
