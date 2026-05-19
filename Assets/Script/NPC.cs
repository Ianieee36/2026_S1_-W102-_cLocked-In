using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping,isDialogueActive;

    private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;

    private void Start()
    {
        dialogueUI = DialogueController.Instance;

        if(dialogueUI == null)
        {
            Debug.LogError("DialogueController.Instance is missing in this scene");
        }
    }
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (dialogueData == null || dialogueUI == null)
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        // Sync with quest data
        SyncQuestState();

        // Set dialogue line based on questState
        if(questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if(questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgressIndex;
        }
        else if(questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);

        DisplayAllCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData is missing.");
            return;
        }

        if (dialogueData.quest == null)
            return;

        if (QuestController.Instance == null)
        {
            Debug.LogError("QuestController.Instance is missing in the scene.");
            return;
        }

        string questID = dialogueData.quest.questId;

        if (QuestController.Instance.isQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }

    void NextLine()
    {
        if (isTyping)
        {   
            //skip typing animation and show the full line
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        // clear choices
        dialogueUI.ClearChoices();

        // check endDialogueLines
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            //If another line, type next line
            DisplayAllCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        // check choices 
        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
           if(dialogueChoice.dialogueIndex == dialogueIndex)
            {   
                Debug.Log("Displaying choices at dialogue index: " + dialogueIndex);
                DisplayChoices(dialogueChoice);
                yield break;
            }
        }

        // stop on end line
        if(dialogueData.endDialogueLines.Length > dialogueIndex &&
           dialogueData.endDialogueLines[dialogueIndex])
        {
            yield break;
        }

        // auto progress
        if(dialogueData.autoProgressLines.Length > dialogueIndex && 
           dialogueData.autoProgressLines[dialogueIndex])
           {
                yield return new WaitForSeconds(dialogueData.autoProgressDelay);
                NextLine();
           } 
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for(int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool givesQuest = choice.givesQuest[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest));
        }   
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {   
        dialogueUI.ClearChoices();

        dialogueIndex = nextIndex;

        if(givesQuest && dialogueData.quest != null) 
        {
            QuestController.Instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
        }
        
        DisplayAllCurrentLine();
    }

    void DisplayAllCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
    }
}
