using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class obstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform minPlantSpawn;
    public Transform maxPlantSpawn;
    public float existenceTime = 4f;
    public float warningDuration = 1.5f;

    [Header("Timing")]
    public float attackCooldown = 6f;
    private float attackTimer;

    [Header("Prefabs")]
    public GameObject warningPrefab;
    public GameObject[] obstacles;

    private GameObject warning;
    void Start()
    {
        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
                StartCoroutine(PerformAoEAttack());
                attackTimer = attackCooldown;
        }
    }

    void OnDisable()
    {
        if (warning != null)
        {
            Destroy(warning);
        }
    }
    IEnumerator PerformAoEAttack()
    {
        float randX = Random.Range(minPlantSpawn.position.x, maxPlantSpawn.position.x);
        float randY = Random.Range(minPlantSpawn.position.y, maxPlantSpawn.position.y);
        Vector3 spawnPos = new Vector3(randX, randY, 0);

        int randIndex = Random.Range(0, obstacles.Length);
        GameObject chosenObstaclePrefab = obstacles[randIndex];
        BoxCollider2D obstacleCollider = chosenObstaclePrefab.GetComponent<BoxCollider2D>();
        warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);

        if (obstacleCollider != null)
        {
            Vector2 trueWorldSize = Vector2.Scale(obstacleCollider.size, chosenObstaclePrefab.transform.localScale);

            warning.transform.localScale = trueWorldSize;
        }
        else
        {
            Debug.LogWarning("The chosen obstacle doesn't have a Collider2D!");
        }

        yield return new WaitForSeconds(warningDuration);

        GameObject obstacle = Instantiate(chosenObstaclePrefab, spawnPos, Quaternion.identity);

        Destroy(warning);
        Destroy(obstacle, existenceTime);
    }
}
