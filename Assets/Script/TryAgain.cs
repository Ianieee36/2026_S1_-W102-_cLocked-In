using UnityEngine;
using TMPro;

public class TryAgain : MonoBehaviour
{
    public static TryAgain Instance;

    [Header("Caught UI")]
    public GameObject caughtPanel;

    [Header("Boss")]
    public BossController boss;

    [Header("Chances")]
    public int maxChances = 3;
    private int currentChances;
    private bool isCaught = false;
    public TextMeshProUGUI chancesText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentChances = maxChances;

        Time.timeScale = 1f;

        if(caughtPanel != null)
        {
            caughtPanel.SetActive(false);
        }

        UpdateChancesText();
    }

    public void PlayerCaught()
    {
        if(isCaught) return;

        isCaught = true;
        currentChances--;

        Time.timeScale = 0f;

        if(caughtPanel != null)
        {
            caughtPanel.SetActive(true);
        }

        UpdateChancesText();
    }

    public void PlayAgain()
    {   
        isCaught = false;
        
        Time.timeScale = 1f;

        if (caughtPanel != null)
            caughtPanel.SetActive(false);
        else
            Debug.LogError("Caught Panel is not assigned.");

        if (boss != null)
            boss.ResetAfterCaught();
        else
            Debug.LogError("Boss is not assigned.");

        if (SaveController.Instance != null)
            SaveController.Instance.LoadGame();
        else
            Debug.LogError("SaveController Instance is missing.");
    }

    void UpdateChancesText()
    {
        if(chancesText != null)
        {
            chancesText.text = "Chances left: " + currentChances;
        }
    }
}
