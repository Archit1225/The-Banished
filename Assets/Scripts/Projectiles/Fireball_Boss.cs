using UnityEngine;

public class Fireball_Boss : MonoBehaviour
{
    public float damage;
    public Animator animator;
    [SerializeField] private float explostionRadius = 2f;
    public LayerMask playerLayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit: " + collision.name + " with tag: " + collision.tag);
        if (collision.CompareTag("Player") || collision.CompareTag("DungeonWalls") || collision.CompareTag("Collisions"))
        {
           GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            Explode();
        }
        else if (collision.CompareTag("Enemy"))
        {
            return;
        }
    }

    void Explode()
    {
        //ExplosionAnimation
        animator.SetTrigger("explode");
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, explostionRadius,playerLayer);

        foreach (Collider2D player in hit)
        {
            player.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
        }
        Destroy(gameObject, 1f);
    }
}
