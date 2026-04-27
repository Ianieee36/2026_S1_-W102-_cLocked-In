using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralSceneTransition : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    private void Start()
    {
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;
    }

    public void GoToScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        float timer = 0f;
        Color c = fadePanel.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;

        SceneManager.LoadScene(sceneName);
    }
}