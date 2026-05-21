using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Time Settings")]
    public float dayLengthInSeconds = 120f;
    public float currentTime = 0f;
    public int currentDay = 1;
    private bool dayStarted = false;
    public bool endOfDayTriggered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        TriggerNewDay();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        currentTime += Time.deltaTime;

        // Trigger end of day at 5PM (progress >= 1)
        if (currentTime >= dayLengthInSeconds && !endOfDayTriggered)
        {
            endOfDayTriggered = true;
            TriggerEndOfDay();
        }
    }

    void TriggerEndOfDay()
    {
        SnakeGame snakeGame = FindObjectOfType<SnakeGame>(true); // true = include inactive
        bool taskCompleted = snakeGame != null && snakeGame.IsTaskCompleted();
        Debug.Log("End of day. Task completed: " + taskCompleted + " snakeGame found: " + (snakeGame != null));

        if (!taskCompleted)
        {
            if (TryAgain.Instance != null)
                TryAgain.Instance.PlayerCaught();
        }
        else
        {
            currentTime = 0f;
            currentDay++;
            endOfDayTriggered = false;
            TriggerNewDay();
        }
    }

    void TriggerNewDay()
    {
        dayStarted = true;
        endOfDayTriggered = false;
        if (DayUI.Instance != null)
            DayUI.Instance.ShowNewDay(currentDay);
    }

    public float GetTimeProgress()
    {
        return currentTime / dayLengthInSeconds;
    }
}