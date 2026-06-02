using UnityEngine;
using System.Collections;
using System.IO;

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
        // Delete existing save so it starts fresh
        string savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (File.Exists(savePath))
            File.Delete(savePath);

        StartCoroutine(NewGameRoutine(sceneName));
    }

    public void ContinueGame()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found, cannot continue.");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
        if (string.IsNullOrEmpty(data.sceneName))
        {
            Debug.LogWarning("Save file has no scene name.");
            return;
        }

        StartCoroutine(ContinueGameRoutine(data.sceneName));
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

    private IEnumerator ContinueGameRoutine(string sceneName)
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