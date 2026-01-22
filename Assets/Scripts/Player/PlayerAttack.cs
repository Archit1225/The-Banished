using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Animator animator;
    public Transform attackPoint;
    public float attackDamage = 30;
    [SerializeField]
    private float attackRange;

    public void AttackDamage()
    {
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemies in hitenemies)
        {
            enemies.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-attackDamage); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        //Gizmos.DrawWireSphere(-attackPoint.position, attackRange);
        //Vector2 attackPositionYP = new Vector2(0, attackPoint.position.x);
        //Vector2 attackPositionYN = new Vector2(0, -attackPoint.position.x);
        //Gizmos.DrawWireSphere(attackPositionYP, attackRange);
        //Gizmos.DrawWireSphere(attackPositionYN, attackRange);
    }
}
