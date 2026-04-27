using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleRectTransform;
    [SerializeField] private Color backgroundActiveColor = Color.green;
    [SerializeField] private Color handleActiveColor = Color.white;

    private Image backgroundImage;
    private Image handleImage;
    private Color backgroundDefaultColor;
    private Color handleDefaultColor;

    private Toggle toggle;
    private Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        if (toggle == null)
            toggle = GetComponentInParent<Toggle>();

        if (toggle == null)
        {
            Debug.LogError("Toggle component not found.");
            return;
        }

        if (uiHandleRectTransform == null)
        {
            Debug.LogError("UI Handle Rect Transform is not assigned.");
            return;
        }

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        if (backgroundImage == null || handleImage == null)
        {
            Debug.LogError("Background or Handle Image is missing.");
            return;
        }

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);
        OnSwitch(toggle.isOn);
    }

    private void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? -handlePosition : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handleActiveColor : handleDefaultColor;
    }

    private void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}