using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string sceneName; 
    public Vector3 playerPosition;
    public Vector3 bossPosition;
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    public List<QuestProgress> questProgressData;
    public List<string> handInQuestIDs;
    public List<string> pickedUpItemIDs;
    public List<string> pickedUpWorldItems;
    public int currentDay;
    public float currentTime;
    public float detection;
    public float detectedTime;
}
