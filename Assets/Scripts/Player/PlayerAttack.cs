using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackDamage = 30;
    public float knockBackTime = 0.5f;
    public float stunTime = 0.3f;
    [SerializeField]
    private float attackRange;

    public void AttackDamage()
    {
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemies in hitenemies)
        {
            enemies.gameObject.GetComponent<Enemy_Health>()?.ChangeHealth(-attackDamage);
            enemies.gameObject.GetComponent<Boss_Health>()?.ChangeHealth(-attackDamage);
            enemies.gameObject.GetComponent<WizardHealth>()?.ChangeHealth(-attackDamage);
            enemies.gameObject.GetComponent<EnemyKnockback>()?.Knockback(transform, knockBackTime, stunTime);
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if(attackPoint == null)
    //    {
    //        return;
    //    }
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}
}
