using UnityEngine;

public class plants_jungle : MonoBehaviour
{
    public float damage = -10f;
    private BoxCollider2D plantCollider;
    public LayerMask playerLayer;

    private void Start()
    {
        plantCollider = GetComponent<BoxCollider2D>();

        Vector2 trueCenter = plantCollider.bounds.center;
        Vector2 trueSize = plantCollider.bounds.size;

        Collider2D[] hit = Physics2D.OverlapBoxAll(trueCenter, trueSize, 0f, playerLayer);

        foreach (Collider2D player in hit)
        {
            player.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(damage);
        }
    }
}
