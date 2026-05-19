using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questId;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    // Called when scriptable obj is edited
    private void OnValidate()
    {
        if(string.IsNullOrEmpty(questId))
        {
            questId = Guid.NewGuid().ToString();
        }
    }
}

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveID; // Match with item ID that you need to collect, NPC id that you need to talk to.
        public string description;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum ObjectiveType { CollectItem, TalkNPC, Custom }

    [System.Serializable]

    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjective> objectives;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjective>();

            foreach(var obj in quest.objectives)
            {
                objectives.Add(new QuestObjective
                {
                    objectiveID = obj.objectiveID,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);

        public string QuestId => quest.questId;
    }
