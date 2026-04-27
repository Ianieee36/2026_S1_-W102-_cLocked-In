using UnityEngine;
using System.Collections;

public class TitleElevatorAnimation : MonoBehaviour
{
    public float bottomOffset = -150f;
    public float topOffset = 150f;

    public float moveDuration = 0.8f;
    public float waitInFrame = 3f;

    private Vector3 middlePosition;
    private Vector3 bottomPosition;
    private Vector3 topPosition;

    void Start()
    {
        // Save the title's current position as the middle/visible position
        middlePosition = transform.localPosition;

        bottomPosition = middlePosition + new Vector3(0, bottomOffset, 0);
        topPosition = middlePosition + new Vector3(0, topOffset, 0);

        // Start visible immediately
        transform.localPosition = middlePosition;

        StartCoroutine(ElevatorCycle());
    }

    IEnumerator ElevatorCycle()
    {
        while (true)
        {
            // Stay visible for 3 seconds first
            yield return new WaitForSeconds(waitInFrame);

            // Move up and disappear
            yield return Move(middlePosition, topPosition);

            // Reset to bottom
            transform.localPosition = bottomPosition;

            // Come back into the middle
            yield return Move(bottomPosition, middlePosition);
        }
    }

    IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(from, to, elapsed / moveDuration);

            yield return null;
        }

        transform.localPosition = to;
    }
}