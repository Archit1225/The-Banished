using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Added to handle Scene changing

public class OldManNPC : MonoBehaviour, IInteractable
{
    /*[Header("UI Elements")]
    public TMP_Text dialogueText, nameText;
    public GameObject dialoguePanel;
    public Image portraitImage;*/
    public Collider2D gateCollider1;
    public AudioClip powerUpAudio;

    [Header("Dialogue States")]
    public Dialogues introDialogue;
    public Dialogues betrayalDialogue;

    private Dialogues currentDialogueData;

    private bool isTyping, isDialogueActive;
    private int dialogueIndex;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            CheckGameStateAndSetDialogue();

            if (currentDialogueData != null)
            {
                StartDialogue();
            }
        }
    }

    private void CheckGameStateAndSetDialogue()
    {
        // If both bosses are dead
        if (GameManager.Instance.hasDefeatedSkeleton && GameManager.Instance.hasDefeatedGoblin)
        {
            currentDialogueData = betrayalDialogue;
        }
        // If no bosses are dead
        else
        {
            currentDialogueData = introDialogue;
            GameManager.Instance.talkedToWizard_1 = true;
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        /*nameText.SetText(currentDialogueData.npcName);
        portraitImage.sprite = currentDialogueData.npcSprite;

        dialoguePanel.SetActive(true);*/
        UI_Controller.instance.SetDialogueBox(currentDialogueData);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            UI_Controller.instance.SetDialogueText(currentDialogueData.dialogueLines[dialogueIndex]);
            isTyping = false; // I added this so skipping a line properly stops the typing state

            if (currentDialogueData.autoProgressLines.Length > dialogueIndex && currentDialogueData.autoProgressLines[dialogueIndex])
            {
                StartCoroutine(AutoProgressDelay());
            }
        }
        else if (++dialogueIndex < currentDialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        UI_Controller.instance.SetDialogueText("");

        foreach (char letter in currentDialogueData.dialogueLines[dialogueIndex])
        {
            UI_Controller.instance.SetDialogueText(UI_Controller.instance.dialogueText.text += letter);
            //dialogueText.text += letter;
            yield return new WaitForSeconds(currentDialogueData.typingSpeed);
        }

        isTyping = false;

        if (currentDialogueData.autoProgressLines.Length > dialogueIndex && currentDialogueData.autoProgressLines[dialogueIndex])
        {
            StartCoroutine(AutoProgressDelay());
        }
    }

    IEnumerator AutoProgressDelay()
    {
        yield return new WaitForSeconds(currentDialogueData.autoProgressDelay);
        NextLine();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        /*UI_Controller.instance.SetDialogueText.SetText("");
        dialoguePanel.SetActive(false);*/
        UI_Controller.instance.CloseDialogueBox();
        if (currentDialogueData == introDialogue)
        {
            //Enable heal UI
            UI_Controller.instance.EnableHealUI();
            AudioManager.instance.PlaySoundFx(powerUpAudio, transform, 0.8f);
            gateCollider1.isTrigger = true;
        }
        if (currentDialogueData == betrayalDialogue)
        {
            SceneManager.LoadScene("DimensionX");
        }
    }
}