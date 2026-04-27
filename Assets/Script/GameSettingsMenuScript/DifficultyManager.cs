using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static string selectedDifficulty = "Intern"; // default

    public void SetDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;

        PlayerPrefs.SetString("Difficulty", difficulty);
        PlayerPrefs.Save();

        Debug.Log("Selected Difficulty: " + difficulty);
    }
}
