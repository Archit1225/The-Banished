using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float asteroidSpeed = 12f;
    public float damage = 15f;

    [HideInInspector] public Vector3 targetPos;

    private PlayerHealth playerHealth;
    private int playerLayerMask;
    private bool isReached=false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerLayerMask = 1 << player.layer;
        }
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, asteroidSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.01f && !isReached)
        {
            Explode();
            isReached = true;
        }
    }

    private void Explode()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.65f, playerLayerMask);

        if (hit != null && playerHealth != null)
        {
            playerHealth.ChangeHealth(-damage);
        }
        anim.SetTrigger("explode");
        Destroy(gameObject, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.65f);
    }
}