using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    public Transform leftDoor; // Reference to the left door transform
    public Transform rightDoor; // Reference to the right door transform

    public float openDistance = 50f; // Distance the doors will move when opening
    public float openDuration = 2.5f; // Duration of the door opening animation

    public SceneTransition sceneTransition; // Reference to the SceneTransition script to handle scene changes
    public string sceneName = "GameSettingsMenu";  // Name of the scene to load when the play button is clicked

    private bool hasClicked = false;

    public void OnPlayClicked() // Method called when the play button is clicked
    {
        if (hasClicked) return;

        hasClicked = true;

        StartCoroutine(OpenDoors());
        sceneTransition.StartTransition(sceneName);
    }

    IEnumerator OpenDoors() // Coroutine to animate the doors opening
    {
        Vector3 leftStart = leftDoor.localPosition; 
        Vector3 rightStart = rightDoor.localPosition;

        Vector3 leftEnd = leftStart + new Vector3(-openDistance, 0, 0); // Move left door to the left
        Vector3 rightEnd = rightStart + new Vector3(openDistance, 0, 0); // Move right door to the right

        float elapsed = 0f;

        while (elapsed < openDuration) // Loop until the doors have fully opened
        {
            elapsed += Time.deltaTime;

            float t = elapsed / openDuration;
            t = t * t * (3f - 2f * t); // Smoothstep interpolation for smoother animation

            leftDoor.localPosition = Vector3.Lerp(leftStart, leftEnd, t); // Move left door towards the left
            rightDoor.localPosition = Vector3.Lerp(rightStart, rightEnd, t); // Move right door towards the right

            yield return null;
        }
    }
}