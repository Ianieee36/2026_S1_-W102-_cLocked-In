using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Time Settings")]
    public float dayLengthInSeconds = 120f; // How long a day lasts in real seconds
    public float currentTime = 0f;
    public int currentDay = 1;

    private bool dayStarted = false;

    void Awake()
    {
        if(Instance == null)
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
        if(Time.timeScale == 0f) return; // Don't tick while paused

        currentTime += Time.deltaTime;

        if(currentTime >= dayLengthInSeconds)
        {
            currentTime = 0f;
            currentDay++;
            TriggerNewDay();
        }
    }

    void TriggerNewDay()
    {
        dayStarted = true;
        if(DayUI.Instance != null)
            DayUI.Instance.ShowNewDay(currentDay);
    }

    // Returns time as a 0-1 value for UI progress bars if needed later
    public float GetTimeProgress()
    {
        return currentTime / dayLengthInSeconds;
    }
}