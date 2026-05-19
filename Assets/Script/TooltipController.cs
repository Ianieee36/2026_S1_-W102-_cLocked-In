using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    public GameObject tooltipPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemIcon;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(string name, string description, Sprite icon)
    {
        itemNameText.text = name;
        itemDescriptionText.text = description;
        itemIcon.sprite = icon;
        itemIcon.preserveAspect = true;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}