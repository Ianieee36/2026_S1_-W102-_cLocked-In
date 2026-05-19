using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{   
    public static QuestController Instance {get; private set; }
    public List<QuestProgress> activateQuests = new();
    private QuestUI questUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        questUI = FindObjectOfType<QuestUI>();
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    public void AcceptQuest(Quest quest)
    {
        if (quest == null) return;

        if (isQuestActive(quest.questId)) return;

        activateQuests.Add(new QuestProgress(quest));

        if (questUI != null)
            questUI.UpdateQuestUI();
        else
            Debug.LogWarning("QuestUI is missing, quest accepted but UI not updated.");
    }

    public bool isQuestActive(string questID) => activateQuests.Exists(q => q != null && q.QuestId == questID);
}
