using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBounce : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;

    public float hoverScale = 1.1f;
    public float clickScale = 0.9f;
    public float speed = 10f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ClickBounce());
    }

    System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        while (Vector3.Distance(transform.localScale, target) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);
            yield return null;
        }
        transform.localScale = target;
    }

    System.Collections.IEnumerator ClickBounce()
    {
        // shrink
        yield return ScaleTo(originalScale * clickScale);

        // bounce back bigger
        yield return ScaleTo(originalScale * hoverScale);

        // return to normal hover
        yield return ScaleTo(originalScale * hoverScale);
    }
}