using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Boss_Health : MonoBehaviour
{
    public float currentHealth;
    private Slider sfxSlider;
    public GameObject bossHealthBar_ParentObject;
    public GameObject pickUpPrefab;
    private float maxHealth;
    private Enemy_Controller controller;
    private Animator bossAnim;
    private EnemyAttributes enemyAttributes;
    [SerializeField] private GameObject aoESpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAnim = GetComponent<Animator>();    
        controller = GetComponent<Enemy_Controller>();
        enemyAttributes = controller.enemyAttributes;
        maxHealth = enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
        Debug.Log("Health bar activated");
        bossHealthBar_ParentObject.GetComponentInChildren<TMP_Text>().SetText(enemyAttributes.name);
        bossHealthBar_ParentObject.SetActive(true);
        sfxSlider = bossHealthBar_ParentObject.GetComponentInChildren<Slider>();
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
            currentHealth = 0;
            OnBossDeath();
        }
    }
    IEnumerator Coroutine_Red()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
        }
    }

    public void OnBossDeath()
    {
        bossAnim.SetTrigger("isDead");
        controller.enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        bossHealthBar_ParentObject.SetActive(false);
        if(aoESpawner!=null) aoESpawner.SetActive(false);
    }

    public void SpawnKey()
    {
        Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
