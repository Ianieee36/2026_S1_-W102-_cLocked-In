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
        if(item != null && TooltipController.Instance != null)
            TooltipController.Instance.ShowTooltip(item.Name, item.Description, itemImage.sprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(TooltipController.Instance != null)
            TooltipController.Instance.HideTooltip();
    }
}