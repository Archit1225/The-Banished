using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Enemy_Health : MonoBehaviour
{
    public float currentHealth;
    public Slider sfxSlider;
    public GameObject bossHealthBar_ParentObject;
    private EnemyType enemyType;
    private float maxHealth;
    private Enemy_Controller controller;
    private EnemyAttributes enemyAttributes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<Enemy_Controller>();
        enemyAttributes = controller.enemyAttributes;
        maxHealth = enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
        enemyType = enemyAttributes.enemyType;
        if(enemyType == EnemyType.Boss)
        {
            Debug.Log("Health bar activated");
            bossHealthBar_ParentObject.GetComponentInChildren<TMP_Text>().SetText(enemyAttributes.name);
            bossHealthBar_ParentObject.SetActive(true);
            sfxSlider = bossHealthBar_ParentObject.GetComponentInChildren<Slider>();
        }
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
        if (health < 0) { StartCoroutine(Coroutine_Red()); }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Play Death Animation
            Wave_Spawner.enemiesAlive--;
            if(enemyType==EnemyType.Boss) //Inactive boss Health Bar
            { bossHealthBar_ParentObject.SetActive(false); }
            Destroy(gameObject);
        }
    }
    IEnumerator Coroutine_Red()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;

    }
}
