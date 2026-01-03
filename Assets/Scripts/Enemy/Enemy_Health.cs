using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    public float currentHealth;
    public Slider sfxSlider;

    private float maxHealth;
    private Enemy_Controller controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<Enemy_Controller>();
        maxHealth = controller.enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (sfxSlider != null)
        {
            sfxSlider.value = currentHealth / maxHealth;
        }
    }
    public void ChangeHealth(float health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Play Death Animation
            Wave_Spawner.enemiesAlive--;
            Destroy(gameObject);
        }
    }
}
