using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TryAgain : MonoBehaviour
{
    public static TryAgain Instance;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("Caught UI")]
    public GameObject caughtPanel;

    [Header("Boss")]
    public BossController boss;
    
    [Header("Chances")]
    public int warningIndex = 0;
    public int maxChances = 3;
    private bool isCaught = false;
    public TextMeshProUGUI chancesText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        warningIndex = 0;

        Time.timeScale = 1f;

        if (caughtPanel != null)
        {
            caughtPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateWarningText();
    }

    public void PlayerCaught()
    {
        if (isCaught) return;

        isCaught = true;

        warningIndex++;

        Time.timeScale = 0f;

        UpdateWarningText();

        // REACHED MAX WARNINGS
        if (warningIndex >= maxChances)
        {
            if (caughtPanel != null)
            {
                caughtPanel.SetActive(false);
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
        else
        {
            if (caughtPanel != null)
            {
                caughtPanel.SetActive(true);
            }
        }
    }

   void UpdateWarningText()
    {
        if (chancesText == null) return;

        string warningMessage = "";

        switch (warningIndex)
        {
            case 1:
                warningMessage = "This is your 1st warning.";
                break;

            case 2:
                warningMessage = "This is your 2nd warning.";
                break;

            case 3:
                warningMessage = "This is your final warning.";
                break;

            default:
                warningMessage = "";
                break;
        }

        chancesText.text = warningMessage;
    }

    public void PlayAgain()
    {
        if (warningIndex >= maxChances)
        {
            Debug.Log("You're Fired.");
            return;
        }

        isCaught = false;
        Time.timeScale = 1f;

        if (caughtPanel != null)
            caughtPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (DayManager.Instance != null)
        {
            DayManager.Instance.currentTime = 0f;
            DayManager.Instance.endOfDayTriggered = false;
        }

        SnakeGame snakeGame = FindObjectOfType<SnakeGame>(true);
        if (snakeGame != null)
            snakeGame.ResetTaskCompletion();

        if (boss != null)
            boss.ResetAfterCaught();

        if (SaveController.Instance != null)
            SaveController.Instance.LoadGame();
        else
            Debug.LogError("SaveController Instance is missing.");
    }

    public void ReturnToMenu(string sceneName)
    {
        StartCoroutine(ReturnToMenuRoutine(sceneName));
    }

    private IEnumerator ReturnToMenuRoutine(string sceneName)
    {
        Time.timeScale = 1f;

        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.StartMenuMusicTransition();
            yield return new WaitForSecondsRealtime(AudioManager.Instance.fadeDuration);
        }

        SceneManager.LoadScene(sceneName);
    }
}
