using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public image portraitImage;

    public int dialogueIndex;
    public bool isTyping,isDialogueActive;
}
