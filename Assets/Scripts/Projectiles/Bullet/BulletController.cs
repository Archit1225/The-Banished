using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public float deleteTimer=10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit: " + collision.name + " with tag: " + collision.tag);
        if (collision.CompareTag("Player")|| collision.CompareTag("DungeonWalls")|| collision.CompareTag("Collisions"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            return;
        }
    }

    private void Update()
    {
        if(deleteTimer > 0)
        {
            deleteTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
