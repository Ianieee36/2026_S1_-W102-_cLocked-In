using UnityEngine;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    public Button internBtn, seniorBtn, ceoBtn;

    public Color selectedColor = Color.blue;
    public Color defaultColor = Color.white;

    public void SelectDifficulty(string difficulty) // Method to update the UI based on the selected difficulty level
    {
        internBtn.image.color = defaultColor;
        seniorBtn.image.color = defaultColor;
        ceoBtn.image.color = defaultColor;

        if (difficulty == "Intern") internBtn.image.color = selectedColor; // Highlight the Intern button if it's the selected difficulty
        if (difficulty == "Senior") seniorBtn.image.color = selectedColor; // Highlight the Senior button if it's the selected difficulty
        if (difficulty == "CEO") ceoBtn.image.color = selectedColor; // Highlight the CEO button if it's the selected difficulty
    }
}