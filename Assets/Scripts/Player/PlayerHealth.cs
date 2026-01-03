using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public Slider sfxSlider;
    public bool isLingering=false;

    private float maxHealth = 1000;
    private bool isAlive = true;
    private float lingeringTimer;
    private Coroutine lingeringCoroutine;
    private WaitForSeconds oneSecond = new WaitForSeconds(1f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        sfxSlider.value = currentHealth/maxHealth;
        if (!isAlive)
        {
            gameObject.SetActive(false);
        }
    }
    public void ChangeHealth(float health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0) {
            currentHealth = 0;
            Debug.Log("Player Died");
            isAlive = false;
        }
    }

    public void LingerHealth(float healthPerSecond, float duration)
    {
        if (lingeringCoroutine != null) {
            StopCoroutine(lingeringCoroutine);
        }
        lingeringCoroutine = StartCoroutine(lingeringRoutine(healthPerSecond, duration));
    }
    private IEnumerator lingeringRoutine(float healthPerSecond, float duration)
    {
        isLingering = true;
        float lingeringTimer = duration;

        while(lingeringTimer >= 0)
        {
            ChangeHealth(healthPerSecond);
            yield return oneSecond;
            lingeringTimer -= 1;
        }
        isLingering = false;
        yield return null;
    }
}
