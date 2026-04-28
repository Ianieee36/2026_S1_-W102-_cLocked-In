using UnityEngine;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    public Button internBtn, seniorBtn, ceoBtn;

    public Color selectedColor = Color.blue;
    public Color defaultColor = Color.white;

    public void SelectDifficulty(string difficulty)
    {
        internBtn.image.color = defaultColor;
        seniorBtn.image.color = defaultColor;
        ceoBtn.image.color = defaultColor;

        if (difficulty == "Intern") internBtn.image.color = selectedColor;
        if (difficulty == "Senior") seniorBtn.image.color = selectedColor;
        if (difficulty == "CEO") ceoBtn.image.color = selectedColor;
    }
}