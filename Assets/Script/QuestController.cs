using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{   
    public static QuestController Instance {get; private set; }
    public List<QuestProgress> activateQuests = new();
    private QuestUI questUI;

    public List<string> handInQuestIDs = new();
    
    private void Start()
    {
        if(InventoryController.Instance != null)
        {
            InventoryController.Instance.OnInventoryChanged += CheckInventoryForQuests;
        }
        else
        {
            Debug.LogError("InventoryController.Instance is missing.");
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
            return;    
        }
        

        questUI = FindObjectOfType<QuestUI>();
    }

    // Update is called once per frame
    public void AcceptQuest(Quest quest)
    {
        if (quest == null) return;

        if (isQuestActive(quest.questId)) return;

        activateQuests.Add(new QuestProgress(quest));

        CheckInventoryForQuests();

        if (questUI != null)
            questUI.UpdateQuestUI();
        else
            Debug.LogWarning("QuestUI is missing, quest accepted but UI not updated.");
    }

    public bool isQuestActive(string questID) => activateQuests.Exists(q => q != null && q.QuestId == questID);

    public void CheckInventoryForQuests()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        foreach(QuestProgress quest in activateQuests)
        {
            foreach(QuestObjective questObjective in quest.objectives)
            {
                if(questObjective.type != ObjectiveType.CollectItem) continue;
                if(!int.TryParse(questObjective.objectiveID, out int itemId)) continue;

                int newAmount = itemCounts.TryGetValue(itemId, out int count) ? Mathf.Min(count, questObjective.requiredAmount) : 0;

                if(questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }

        if(questUI != null)
        {
            questUI.UpdateQuestUI();    
        }
        
    }

    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestId == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted);
    }

    public void HandInQuest(string questID)
    {
        Debug.Log("Trying to hand in quest: " + questID);

        QuestProgress quest = activateQuests.Find(q => q.QuestId == questID);

        if (quest == null)
        {
            Debug.LogWarning("Quest not found in active quests: " + questID);
            return;
        }

        if (!IsQuestCompleted(questID))
        {
            Debug.LogWarning("Quest is not completed yet: " + questID);
            return;
        }

        if (!RemoveRequiredItemsFromInventory(questID))
        {
            Debug.LogWarning("Could not remove required items for quest: " + questID);
            return;
        }

        handInQuestIDs.Add(questID);
        activateQuests.Remove(quest);

        Debug.Log("Quest handed in and removed: " + questID);

        if (questUI != null)
            questUI.UpdateQuestUI();
    }

    public bool IsQuestHandedIn(string questID)
    {
        return handInQuestIDs.Contains(questID);
    }

    public bool RemoveRequiredItemsFromInventory(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestId == questID);
        if(quest == null) return false;

        Dictionary<int, int> requiredItems = new();

        // Item requirements from objectives
        foreach(QuestObjective objective in quest.objectives)
        {
            if(objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemID))
            {
                requiredItems[itemID] = objective.requiredAmount;
            }
        }

        // Verify we have items
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(var item in requiredItems)
        {
            if(itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                // Not enough items to complete quest
                return false;
            }
        }

        // Removed required items from inventory
        foreach(var itemRequirement in requiredItems)
        {
            // RemovedItemsFromInventory
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }

        return true;
    }

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();

        CheckInventoryForQuests();

        if(questUI != null)
        {
            questUI.UpdateQuestUI();    
        }
        
    }
}
