using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsButton : MonoBehaviour
{   
    public GeneralSceneTransition generalSceneTransition; // transition scene

    // CEO Difficulty
    public void SelectCEO()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.CEO);
    }

    // Senior Difficulty
    public void SelectSenior()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.Senior);
    }

    // Intern Difficulty
    public void SelectIntern()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.Intern);
    }

    // New Game Scene Load
    public void NewGame(string sceneName)
    {
        if (generalSceneTransition == null)
        {
            Debug.LogError("Scene Transition is not assigned in GameSettingsButton.");
            return;
        }
        generalSceneTransition.LoadSceneWithFade(sceneName);
    }
}
