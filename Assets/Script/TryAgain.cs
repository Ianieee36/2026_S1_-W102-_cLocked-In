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

        if (DayManager.Instance != null)
        {
            DayManager.Instance.currentTime = 0f;
            DayManager.Instance.endOfDayTriggered = false;
        }

        SnakeGame snakeGame = FindObjectOfType<SnakeGame>(true);
        if (snakeGame != null)
            snakeGame.ResetTaskCompletion();

        if (caughtPanel != null)
            caughtPanel.SetActive(false);
        if (boss != null)
            boss.ResetAfterCaught();
        if (SaveController.Instance != null)
            SaveController.Instance.LoadGame();
    }

    void UpdateChancesText()
    {
        if(chancesText != null)
        {
            chancesText.text = "Chances left: " + currentChances;
        }
    }
}
