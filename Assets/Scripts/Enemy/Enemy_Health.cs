using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_Health : MonoBehaviour
{
    public float currentHealth;
    public Slider sfxSlider;
    private EnemyType enemyType;
    public float maxHealth;
    private Enemy_Controller controller;
    private Animator animator;
    private EnemyAttributes enemyAttributes;
    private DamageFlash flashEffect;
    private bool isMinion = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();    
        controller = GetComponent<Enemy_Controller>();
        flashEffect = GetComponent<DamageFlash>();
        enemyAttributes = controller.enemyAttributes;
        if (!isMinion)
        {
            maxHealth = enemyAttributes.enemy_Health;
            currentHealth = maxHealth;
        }
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
        if (health < 0) { flashEffect.Flash(); }
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
        }
    }

    public void SetSpiritHealth()
    {
        isMinion = true;
        maxHealth = 1f;
        currentHealth = 1f;

        if (sfxSlider != null)
        {
            sfxSlider.value = 1f;
        }
    }
}
