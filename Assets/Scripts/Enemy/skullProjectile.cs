using System.Collections;
using UnityEngine;

public class SkullAttack : MonoBehaviour
{
    [Header("Settings")]
    public float roamSpeed = 3f;
    public float dashSpeed = 20f;
    public float roamRadius = 4f;
    public int minRoams = 1;
    public int maxRoams = 6;
    public float damage = 10f;
    public Transform player;

    private Vector2 targetPosition;
    private bool isDashing = false;
    void Start()
    {
        targetPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SkullSequence());//Not putting it in Update it will crash then
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Damage Player
            player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            //Breaking Animation
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float currentSpeed = isDashing ? dashSpeed : roamSpeed;//Handling Speed
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        if (isDashing && (Vector2)transform.position == targetPosition)
        {
            //Skull has done his job now destroy
            //Or Explode
            Destroy(gameObject, 1);    
        }
    }

    IEnumerator SkullSequence()
    {
        int roamCount = Random.Range(minRoams, maxRoams);

        for (int i = 0; i < roamCount; i++)
        {
            targetPosition = (Vector2)transform.position + Random.insideUnitCircle * roamRadius;

            yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPosition) < 0.1f);//WaitUntill requires such bool returning function to work 

            yield return new WaitForSeconds(0.2f);//Pause for a bit to create anticipation
        }

        targetPosition = transform.position; 
        //Maybe an animation indicating its going to attack but it will kill the suspense 
        yield return new WaitForSeconds(0.75f); 

        if (player != null)
        {
            // Lock onto where the player is standing 
            targetPosition = player.position;
            isDashing = true;
        }
    }
}
