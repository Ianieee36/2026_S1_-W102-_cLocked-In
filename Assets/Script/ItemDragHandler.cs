using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 1f;
    public float maxDropDistance = 3f;

    public static bool isDraggingItem = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraggingItem = true;
        originalParent = transform.parent; //Save OG parent
        transform.SetParent(transform.root); //Above other canvas
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //Follows the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraggingItem = false;
        canvasGroup.blocksRaycasts = true; //Enable raycasts
        canvasGroup.alpha = 1f; //no longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //Slot where item is dropped
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if(dropSlot != null)
        {
            if(dropSlot.currentItem != null)
            {
                //If the slot has an item, then swap them
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            } else
            {
                originalSlot.currentItem = null;
            }
            //Move item into dropped slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        } else
        {
            //If where were dropping isnt within the inventory
            //Drops our item outside of our inventory
            if(!IsWithinInventory(eventData.position))
            {
                if(IsFromHotbar())
                {
                    DropItemAtMouse(originalSlot, eventData.position);
                }
                else
                {
                    DropItem(originalSlot);
                }
            }
            else
            {
                //No slot under drop point, goes back to OG slot
                transform.SetParent(originalParent);
            }    
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Centers it into the slot
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
       RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }

        //Random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        //Instantiate drop item
        Instantiate(gameObject, dropPosition, Quaternion.identity);

        //Destroy the UI one
        Destroy(gameObject);

    }

    bool IsFromHotbar()
    {
        return originalParent.parent.name == "Hotbar";
    }

    void DropItemAtMouse(Slot originalSlot, Vector2 mouseScreenPosition)
    {
        originalSlot.currentItem = null;

        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(mainCamera.transform.position.z))
        );

        Vector2 dropPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        Instantiate(gameObject, dropPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
