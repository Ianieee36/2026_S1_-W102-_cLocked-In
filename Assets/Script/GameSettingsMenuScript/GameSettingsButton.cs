using UnityEngine;
using System.Collections;

public class GameSettingsButton : MonoBehaviour
{
    public GeneralSceneTransition generalSceneTransition;

    public void SelectCEO()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.CEO);
    }

    public void SelectSenior()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.Senior);
    }

    public void SelectIntern()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.Difficulty.Intern);
    }

    public void NewGame(string sceneName)
    {
        StartCoroutine(NewGameRoutine(sceneName));
    }

    private IEnumerator NewGameRoutine(string sceneName)
    {
        if (generalSceneTransition == null)
        {
            Debug.LogError("Scene Transition is not assigned in GameSettingsButton.");
            yield break;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartGameplayMusic();
            yield return new WaitForSeconds(AudioManager.Instance.fadeDuration);
        }

        generalSceneTransition.LoadSceneWithFade(sceneName);
    }
}