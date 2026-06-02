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
            pickedUpWorldItems = WorldItemSaveData.GetAllPickedUp(),
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, // added
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            bossPosition = GameObject.FindGameObjectWithTag("BossCharacter").transform.position,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            questProgressData = QuestController.Instance.activateQuests,
            handInQuestIDs = QuestController.Instance.handInQuestIDs,
            detection = GameObject.FindGameObjectWithTag("BossCharacter")?.GetComponent<BossController>()?.detection ?? 0f,
            detectedTime = GameObject.FindGameObjectWithTag("BossCharacter")?.GetComponent<BossController>()?.detectedTime ?? 0f,
            currentDay = DayManager.Instance != null ? DayManager.Instance.currentDay : 1,
            currentTime = DayManager.Instance != null ? DayManager.Instance.currentTime : 0f
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            WorldItemSaveData.LoadPickedUp(saveData.pickedUpWorldItems);

            // Tell all world items to check if they were picked up
            foreach (WorldItem worldItem in FindObjectsOfType<WorldItem>(true))
                worldItem.CheckIfPickedUp();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject boss = GameObject.FindGameObjectWithTag("BossCharacter");
            BossController bossController = boss?.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.detection = saveData.detection;
                bossController.detectedTime = saveData.detectedTime;
            }

            if (player != null) player.transform.position = saveData.playerPosition;
            if (boss != null) boss.transform.position = saveData.bossPosition;

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestController.Instance.handInQuestIDs = saveData.handInQuestIDs;
            if (DayManager.Instance != null)
            {
                DayManager.Instance.currentDay = saveData.currentDay;
                DayManager.Instance.currentTime = saveData.currentTime;
            }
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