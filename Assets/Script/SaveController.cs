using UnityEngine;
using System.IO;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, // added
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            bossPosition = GameObject.FindGameObjectWithTag("BossCharacter").transform.position,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            questProgressData = QuestController.Instance.activateQuests,
            handInQuestIDs = QuestController.Instance.handInQuestIDs
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if(File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            GameObject.FindGameObjectWithTag("BossCharacter").transform.position = saveData.bossPosition;

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestController.Instance.handInQuestIDs = saveData.handInQuestIDs;
        }
        else
        {
            SaveGame();
        }
    }

    public bool HasSaveFile() // added
    {
        return File.Exists(saveLocation);
    }
}