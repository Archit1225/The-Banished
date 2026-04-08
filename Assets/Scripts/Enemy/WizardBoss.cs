using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

public class WizardBoss : MonoBehaviour
{
    public EnemyAttributes enemyAttributes;
    public List<Attacks> attackMoveset; //Stores all the moveset enemy can use
    public Animator animator;
    private float contactDamage = 10f;
    public float chaseSpeed; //Speed of enemy
    public float attackRange = 1.5f; //Will be use for OverLap Circle
    //public Transform attackPoint; //Will be used for Overlap Circle
    //public Transform enemyFacePos;
    public LayerMask playerLayer;

    public GameObject player;
    private PlayerHealth playerHealth;
    private Transform playerPos;

    private Rigidbody2D rb;
    public EnemyStates currentState; //Stores the current state of the enemy
    private Attacks attackPerforming; //Stores the attack the enemy is going to perform
    private float distance; //Stores the distance between player and enemy
    private Vector2 direction; //Stores the direction between player and enemy
    private bool isAttackOnCooldown; //Stores whether the enemy is on cooldown or not
    private float MaxRange = 0;
    private Vector2 lastLookDir;
    private List<Attacks> potentialAttacks = new List<Attacks>();

    //EnemySpawning
    public GameObject[] enemyPrefabs;

    //Asteroid Spawning
    public Transform minSpawnPos;
    public Transform maxSpawnPos;
    public GameObject warningPrefab;
    public GameObject asteroidPrefab;
    public float warningDuration = 1f;
    public int asteroidNumber=10;

    //Asteroid Target
    public Transform minTargetPos;
    public Transform maxTargetPos;

    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.ChangeHealth(contactDamage);
            player.GetComponent<PlayerMovement>().Knockback(transform, 5, 1.5f);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerPos = player.transform;
        ChangeState(EnemyStates.Chasing);
        distance = Vector2.Distance(transform.position, playerPos.position);
        direction = (playerPos.position - transform.position).normalized;
        chaseSpeed = enemyAttributes.enemy_Speed;

        foreach (Attacks attack in attackMoveset)
        {
            if (attack.maxRange > MaxRange)
            {
                MaxRange = attack.maxRange;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindPositionOfPlayer();
        if (currentState != EnemyStates.Knockback)
        {
            switch (currentState)
            {
                case EnemyStates.Idle:
                    Debug.Log("Idle");
                    rb.linearVelocity = Vector2.zero;
                    break;
                case EnemyStates.Chasing:
                    Debug.Log("Chasing");
                    ChasePlayer();
                    break;
                case EnemyStates.Attacking:
                    if (!isAttackOnCooldown)
                    {
                        RandomizeAttack();
                    }
                    break;
            }
        }
    }

    private void FindPositionOfPlayer()
    {
        if (playerPos != null)
        {
            distance = Vector2.Distance(transform.position, playerPos.position);
            direction = (playerPos.position - transform.position).normalized;

            animator.SetFloat("LookDirX", direction.x);
            animator.SetFloat("LookDirY", direction.y);
        }
    }

    private void ChasePlayer() //Chases PLayer + Also holds logic for whether the enemy should attack the player 
    {
        if (distance < MaxRange)
        {
            ChangeState(EnemyStates.Attacking);
            return;
        }
        Debug.Log("Moving");
        transform.position = Vector2.MoveTowards(transform.position, playerPos.position, chaseSpeed * Time.deltaTime);
        Debug.Log("Moving2");
    }

    public void RandomizeAttack()
    {
        potentialAttacks.Clear();
        foreach (Attacks attack in attackMoveset)
        {
            if (distance < attack.maxRange && distance >= attack.minRange)
            {
                Debug.Log("Attack");
                potentialAttacks.Add(attack);
            }
        }

        if (potentialAttacks.Count > 0)
        {
            isAttackOnCooldown = true;
            Debug.Log("Size of potential Attacks = " + potentialAttacks.Count);
            int randomIndex = Random.Range(0, potentialAttacks.Count);
            Debug.Log("Random index = " + randomIndex);
            attackPerforming = potentialAttacks[randomIndex];
            StartCoroutine(AnimateAttack(attackPerforming));
        }
        else
        {
            ChangeState(EnemyStates.Chasing);
        }
    }

    private IEnumerator AnimateAttack(Attacks attack) //This controls animation part of the attack
    {
        lastLookDir = direction;
        Vector2 temp = direction;
        temp = SetAnimatorDirection(temp);
        animator.SetFloat("LastLookDirX", temp.x);
        animator.SetFloat("LastLookDirY", temp.y);

        animator.SetTrigger(attackPerforming.animationTrigger);

        Debug.Log(attackPerforming.AttackName);

        ChangeState(EnemyStates.Idle);

        yield return new WaitForSeconds(attack.attackCooldown);

        isAttackOnCooldown = false;
        Debug.Log("CoolDown Over");
        HandleIdle(); //Goes back to Chasing 
    }

    public void SkullShoot()
    {
        if (attackPerforming.attackAudio != null)
        {
            AudioManager.instance.PlaySoundFx(attackPerforming.attackAudio, transform, 0.2f);
        }
        for (int i = -1; i <= 1; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(i, 1, 0);
            GameObject projectile = Instantiate(attackPerforming.projectilePrefab, spawnPos, Quaternion.identity);
        }
    }
    public IEnumerator SpawnAsteroidCoroutine()
    {
        //Choose the targetPos - Player Pos
        //Vector3 targetPos = playerPos.position;
        List<Vector3> targetPositions = new List<Vector3>();
        List<Vector3> spawnPositions = new List<Vector3>();
        List<GameObject> activeWarnings = new List<GameObject>();
        Vector3 trueWorldSize = new Vector3();

        CircleCollider2D obstacleCollider = asteroidPrefab.GetComponent<CircleCollider2D>();
        if (obstacleCollider != null)
        {
            float diameter = obstacleCollider.radius * 2f;
            Vector2 localSize = new Vector2(diameter, diameter);

            trueWorldSize = Vector2.Scale(localSize, asteroidPrefab.transform.localScale);
        }

        for (int i = 1; i <= asteroidNumber; i++)
        {
            GameObject warning;
            float targetX = Random.Range(minTargetPos.position.x, maxTargetPos.position.x);
            float targetY = Random.Range(minTargetPos.position.y, maxTargetPos.position.y);
            Vector3 targetPos = new Vector3(targetX, targetY, 0);

            float randX = Random.Range(minSpawnPos.position.x, maxSpawnPos.position.x);
            float randY = Random.Range(minSpawnPos.position.y, maxSpawnPos.position.y);
            Vector3 spawnPos = new Vector3(randX, randY, 0);

            warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);
            warning.transform.localScale = trueWorldSize;
            targetPositions.Add(targetPos);
            spawnPositions.Add(spawnPos);
            activeWarnings.Add(warning);
        }

        yield return new WaitForSeconds(warningDuration);
        for (int i = 0; i < activeWarnings.Count; i++)
        {
            Destroy(activeWarnings[i]);
            GameObject asteroidInstance = Instantiate(asteroidPrefab, spawnPositions[i], Quaternion.identity);
            if (spawnPositions[i].x > targetPositions[i].x)
            {
                asteroidInstance.transform.localScale *= -1;
                Debug.Log("On your Right");
            }
            asteroidInstance.GetComponent<Asteroid>().targetPos = targetPositions[i];
        }
    }

    public void SpawnEnemy()
    {
        int randIndex = Random.Range(0, enemyPrefabs.Length);

        //Spawn Enemy and get back
        GameObject enemy = Instantiate(enemyPrefabs[randIndex], transform.position, Quaternion.identity);
        SpriteRenderer sprite = enemy.GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        enemy.GetComponent<Enemy_Health>().currentHealth = 1;
    }
    private Vector2 SetAnimatorDirection(Vector2 dir)
    {
        float x = 0;
        float y = 0;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            x = dir.x > 0 ? 1f : -1f;
            y = 0f;
        }
        else
        {
            y = dir.y > 0 ? 1f : -1f;
            x = 0f;
        }
        return new Vector2(x, y);
    }

    private void HandleIdle()
    {
        if (playerPos != null)
        {
            if (distance > MaxRange)
                ChangeState(EnemyStates.Chasing);
            else
                ChangeState(EnemyStates.Attacking);
        }
    }

    public void ChangeState(EnemyStates newState)
    {
        // Reset all bools first
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);

        // Set the correct one
        switch (newState)
        {
            case EnemyStates.Idle: animator.SetBool("isIdle", true); break;
            case EnemyStates.Chasing: animator.SetBool("isChasing", true); break;
            case EnemyStates.Attacking: animator.SetBool("isAttacking", true); break;
        }

        currentState = newState;
    }

    public EnemyStates GetEnemyState()
    {
        return currentState;
    }

}

