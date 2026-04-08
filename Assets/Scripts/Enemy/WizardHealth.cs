using UnityEngine;
using System.Collections;

public class WizardHealth : MonoBehaviour
{
    public float currentHealth;
    //private Slider sfxSlider;
    //public GameObject bossHealthBar_ParentObject;
    public GameObject pickUpPrefab;
    public AudioClip wizardHurtSound;
    private float maxHealth;
    private WizardBoss wizardController;
    private Animator bossAnim;
    private EnemyAttributes enemyAttributes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAnim = GetComponent<Animator>();
        wizardController = GetComponent<WizardBoss>();
        enemyAttributes = wizardController.enemyAttributes;
        maxHealth = enemyAttributes.enemy_Health;
        currentHealth = maxHealth;
        Debug.Log("Health bar activated");
        UI_Controller.instance.EnableBossUI(enemyAttributes.enemy_Name);
    }
    public void ChangeHealth(float health)
    {
        currentHealth += health;
        if (health < 0) { 
            StartCoroutine(Coroutine_Red());
            AudioManager.instance.PlaySoundFx(wizardHurtSound, transform, 0.7f);
        }
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnBossDeath();
        }
        UI_Controller.instance.HealthbarFillerBoss(currentHealth, maxHealth);
    }
    IEnumerator Coroutine_Red()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
        }
    }

    public void OnBossDeath()
    {
        bossAnim.SetTrigger("isDead");
        wizardController.enabled = false;
        GameManager.Instance.hasDefeatedWizard = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        UI_Controller.instance.DisableBossUI();
    }

    public void SpawnKey()
    {
        Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
