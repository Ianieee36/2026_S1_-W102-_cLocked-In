using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    // Uses an enum for the difficulty
    public enum Difficulty
    {
        Intern,
        Senior,
        CEO
    }

    public Difficulty currentDifficulty;

    // hides fields from inspector
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float chaseSpeed;
    [HideInInspector] public float visionRange;
    [HideInInspector] public float detectionRate;
    [HideInInspector] public float decayRate;
    [HideInInspector] public float timeToLose;

    //
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplyDifficulty();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Selected Difficulty
    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        ApplyDifficulty();

        Debug.Log("Selected Difficulty: " + currentDifficulty);
    }

    // Difficulty is applied based on the player prefs.
    void ApplyDifficulty()
    {
        switch(currentDifficulty)
        {
            // Intern initial state difficulty
            case Difficulty.Intern:
                moveSpeed = 1.5f;
                chaseSpeed = 2f;
                visionRange = 3.8f;
                detectionRate = 0.5f;
                decayRate = 0.6f;
                timeToLose = 5f;
                break;

            // Senior initial state difficulty
            case Difficulty.Senior:
                moveSpeed = 1.5f;
                chaseSpeed = 2.5f;
                visionRange = 3.8f;
                detectionRate = 2f;
                decayRate = 0.2f;
                timeToLose = 4f;
                break;

            // CEO initial state difficulty
            case Difficulty.CEO:
                moveSpeed = 1.5f;
                chaseSpeed = 3f;
                visionRange = 3.8f;
                detectionRate = 0f;
                decayRate = 0.05f;
                timeToLose = 3f;
                break;
        }
    }
}