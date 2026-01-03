using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
