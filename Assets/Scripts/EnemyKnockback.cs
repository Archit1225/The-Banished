using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Controller enemy_Controller;
    private Coroutine knockbackRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Controller = GetComponent<Enemy_Controller>();    
    }

    public void Knockback(Transform playerTransform, float knockBackForce, float knockbackTime, float stunTime)
    {
        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);    
        }
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * knockBackForce;
        //This avoids using of string which can be difficult if you rename it and also acts as an instance
        knockbackRoutine = StartCoroutine(StunTimer(knockbackTime, stunTime));
        knockbackRoutine = null;
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        enemy_Controller.ChangeState(EnemyStates.Knockback);
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemy_Controller.ChangeState(EnemyStates.Chasing);
    }
}
