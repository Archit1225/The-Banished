using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_Health : MonoBehaviour
{
    public float currentHealth;
    public Slider sfxSlider;
    private EnemyType enemyType;
    private float maxHealth;
    private Enemy_Controller controller;
    private Animator animator;
    private EnemyAttributes enemyAttributes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();    
        controller = GetComponent<Enemy_Controller>();
        enemyAttributes = controller.enemyAttributes;
        maxHealth = enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
        enemyType = enemyAttributes.enemyType;
    }
    void HealthbarFiller()
    {
        float ratio = currentHealth / maxHealth;
        if (sfxSlider != null)
        {
            sfxSlider.value = Mathf.Lerp(sfxSlider.value, ratio, 0.1f);
        }
    }
    private void Update()
    {
        HealthbarFiller();
    }
    public void ChangeHealth(float health)
    {
        currentHealth += health;
        if (health < 0) { StartCoroutine(Coroutine_Red()); }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            if (!controller.isActiveAndEnabled) { return; }
            currentHealth = 0;
            //Play Death Animation
            animator.ResetTrigger(controller.attackPerforming.animationTrigger);
            animator.SetTrigger("Death");
            if (FindAnyObjectByType<Wave_Spawner>() != null)
            {
                Wave_Spawner.enemiesAlive--;
            }
            controller.enabled = false;
            Destroy(gameObject, 1.5f);
        }
    }
    IEnumerator Coroutine_Red()
    {
        Color originalColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = originalColor;
    }
}
