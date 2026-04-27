using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel;
    public float elevatorOpenDelay = 2f;
    public float fadeDuration = 1.5f;

    public void StartTransition(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        // wait for elevator opening animation/sound
        yield return new WaitForSeconds(elevatorOpenDelay);

        // fade to black
        float timer = 0f;
        Color color = fadePanel.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = 1f;
        fadePanel.color = color;

        SceneManager.LoadScene(sceneName);
    }
}