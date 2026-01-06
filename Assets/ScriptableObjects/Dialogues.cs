using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogues", menuName = "NPC Dialogue")]
public class Dialogues : ScriptableObject
{
    public string npcName;
    public Sprite npcSprite;
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
}
