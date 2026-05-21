using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item item;
    private Image itemImage;

    void Start()
    {
        item = GetComponent<Item>();
        itemImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovering over: " + (item != null ? item.Name : "null item") + " desc: " + (item != null ? item.Description : "null desc"));
        if (item != null && TooltipController.Instance != null)
            TooltipController.Instance.ShowTooltip(item.Name, item.Description, itemImage.sprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(TooltipController.Instance != null)
            TooltipController.Instance.HideTooltip();
    }
}