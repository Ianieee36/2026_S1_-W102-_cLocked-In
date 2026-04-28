using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public float openDistance = 50f;
    public float openDuration = 2.5f;

    public SceneTransition sceneTransition;
    public string sceneName = "GameSettingsMenu";

    private bool hasClicked = false;

    public void OnPlayClicked()
    {
        if (hasClicked) return;

        hasClicked = true;

        StartCoroutine(OpenDoors());
        sceneTransition.StartTransition(sceneName);
    }

    IEnumerator OpenDoors()
    {
        Vector3 leftStart = leftDoor.localPosition;
        Vector3 rightStart = rightDoor.localPosition;

        Vector3 leftEnd = leftStart + new Vector3(-openDistance, 0, 0);
        Vector3 rightEnd = rightStart + new Vector3(openDistance, 0, 0);

        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / openDuration;
            t = t * t * (3f - 2f * t);

            leftDoor.localPosition = Vector3.Lerp(leftStart, leftEnd, t);
            rightDoor.localPosition = Vector3.Lerp(rightStart, rightEnd, t);

            yield return null;
        }
    }
}