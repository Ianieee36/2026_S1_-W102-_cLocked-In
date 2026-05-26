using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGame : MonoBehaviour
{
    [Header("Scene Transition")]
    public GeneralSceneTransition generalSceneTransition;

    public void ReturnToMenu(string sceneName)
    {
        StartCoroutine(ReturnToMenuRoutine(sceneName));
    }

    private IEnumerator ReturnToMenuRoutine(string sceneName)
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartMenuMusicTransition();
        }

        if (generalSceneTransition != null)
        {
            generalSceneTransition.LoadSceneWithFade(sceneName);
        }
        else
        {
            Debug.LogWarning("GeneralSceneTransition is not assigned. Loading without fade.");
            SceneManager.LoadScene(sceneName);
        }

        yield return null;
    }
}