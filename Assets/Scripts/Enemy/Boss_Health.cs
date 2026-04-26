using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Boss_Health : MonoBehaviour
{
    public float currentHealth;
    //private Slider sfxSlider;
    //public GameObject bossHealthBar_ParentObject;
    public GameObject pickUpPrefab;
    public AudioClip hurtSound;
    private float maxHealth;
    private Enemy_Controller controller;
    private Animator bossAnim;
    private EnemyAttributes enemyAttributes;
    private DamageFlash flashEffect;
    [SerializeField] private GameObject aoESpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAnim = GetComponent<Animator>();    
        controller = GetComponent<Enemy_Controller>();
        flashEffect = GetComponent<DamageFlash>();
        enemyAttributes = controller.enemyAttributes;
        maxHealth = enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
        Debug.Log("Health bar activated");
        /*bossHealthBar_ParentObject.SetActive(true);
        bossHealthBar_ParentObject.GetComponentInChildren<TMP_Text>().SetText(enemyAttributes.name);
        sfxSlider = bossHealthBar_ParentObject.GetComponentInChildren<Slider>();*/
        UI_Controller.instance.EnableBossUI(enemyAttributes.enemy_Name);
    }
    /*void HealthbarFiller()
    {
        float ratio = currentHealth / maxHealth;
        if (sfxSlider != null)
        {
            sfxSlider.value = Mathf.Lerp(sfxSlider.value, ratio, 0.1f);
        }
    }*/
    private void Update()
    {
        UI_Controller.instance.HealthbarFillerBoss(currentHealth, maxHealth);
    }
    public void ChangeHealth(float health)
    {
        currentHealth += health;
        if (health < 0) { 
            flashEffect.Flash();
            AudioManager.instance.PlaySoundFx(hurtSound, transform, 0.8f);
        }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            if (!controller.isActiveAndEnabled) { return; }
            currentHealth = 0;
            OnBossDeath();
        }
    }

    public void OnBossDeath()
    {
        bossAnim.SetTrigger("isDead");
        switch (enemyAttributes.enemy_Name)
        {
            case "Skeleton King":
                GameManager.Instance.hasDefeatedSkeleton = true;
                break;
            case "Goblin Beast":
                GameManager.Instance.hasDefeatedGoblin = true;
                break;
        }
        controller.enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //bossHealthBar_ParentObject.SetActive(false);
        UI_Controller.instance.DisableBossUI();
        if(aoESpawner!=null) aoESpawner.SetActive(false);
    }

    public void SpawnKey()
    {
        Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
