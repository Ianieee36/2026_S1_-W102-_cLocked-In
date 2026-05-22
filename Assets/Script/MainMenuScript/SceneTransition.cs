using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel; // Reference to the UI Image used for fading effect
    public float elevatorOpenDelay = 2f; // Time to wait for the elevator opening animation/sound before starting the fade
    public float fadeDuration = 1.5f; // Duration of the fade to black effect
    public GeneralSceneTransition generalSceneTransition;

    public void StartTransition(string sceneName) // Method to initiate the scene transition process
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName) // Coroutine to handle the scene transition with fade effect
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