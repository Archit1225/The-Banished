using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;
    public GameObject gameOverPanel;

    public GameObject bossHealthBar_ParentObject;
    private Slider enemyHealthBar;

    [Header("Player UI")]
    public Slider healUI;
    public Slider playerHealthBar;
    public GameObject playerUI;

    [Header("UI Elements")]
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject dialoguePanel;
    public Image portraitImage;

    private Coroutine playerHealthCoroutine;
    private Coroutine enemyHealthCoroutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        playerUI.SetActive(false);
        //Pause The Game
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false);
        DisableBossUI();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    //BOSS UI
    public void EnableBossUI(string bossName)
    {
        bossHealthBar_ParentObject.SetActive(true);
        bossHealthBar_ParentObject.GetComponentInChildren<TMP_Text>().SetText(bossName);
        enemyHealthBar = bossHealthBar_ParentObject.GetComponentInChildren<Slider>();
    }
    public void DisableBossUI()
    {
        bossHealthBar_ParentObject.SetActive(false);
    }

    //SLIDERS
    public void HealthbarFillerBoss(float currentHealth, float maxHealth)
    {
        float ratio = currentHealth / maxHealth;
        if (enemyHealthCoroutine != null)
        {
            StopCoroutine(enemyHealthCoroutine);
        }
        enemyHealthCoroutine = StartCoroutine(SmoothLerpCoroutine(enemyHealthBar, ratio, 0.5f));
    }

    public void UpdateHealCooldown(float healTimer, float healCooldown)
    {
        if (healUI != null) healUI.value = healTimer / healCooldown;
    }
    public void EnableHealUI()
    {
        healUI.gameObject.SetActive(true);
    }
    public void HealthbarFillerPlayer(float currentHealth, float maxHealth)
    {
        float ratio = currentHealth / maxHealth;
        if (playerHealthCoroutine != null)
        {
            StopCoroutine(playerHealthCoroutine);
        }
        playerHealthCoroutine = StartCoroutine(SmoothLerpCoroutine(playerHealthBar, ratio, 0.5f));
    }

    private IEnumerator SmoothLerpCoroutine(Slider sliderToMove, float targetValue, float duration)
    {
        float timePassed = 0f;
        float startValue = sliderToMove.value;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            sliderToMove.value = Mathf.Lerp(startValue, targetValue, timePassed / duration);
            yield return null;
        }
        sliderToMove.value = targetValue;
    }
    //DIALOGUE BOX
    public void SetDialogueBox(Dialogues dialogueData)
    {
        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcSprite;
        dialoguePanel.SetActive(true);
    }

    public void SetDialogueText(string text)
    {
        dialogueText.SetText(text);
    }

    public void CloseDialogueBox()
    {
        SetDialogueText("");
        dialoguePanel.SetActive(false);
    }
}
