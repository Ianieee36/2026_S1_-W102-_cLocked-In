using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static string selectedDifficulty = "Intern"; // Default difficulty level

    public void SetDifficulty(string difficulty) // Method to set the selected difficulty level
    {
        selectedDifficulty = difficulty; // Update the static variable with the selected difficulty

        PlayerPrefs.SetString("Difficulty", difficulty); // Save the selected difficulty level to PlayerPrefs for persistence
        PlayerPrefs.Save(); // Ensure that the PlayerPrefs are saved to disk

        Debug.Log("Selected Difficulty: " + difficulty); // Log the selected difficulty level for debugging purposes
    }
}
