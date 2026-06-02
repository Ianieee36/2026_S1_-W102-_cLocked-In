using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public int itemID;          // Set this in Inspector for each world item
    public Sprite itemSprite;   // Set this to show the right sprite
    public string worldItemID;
    private bool hasBeenPickedUp = false;

    void Start()
    {
        if(string.IsNullOrEmpty(worldItemID))
            worldItemID = GlobalHelper.GenerateUniqueID(gameObject);

        // Set sprite
        if(itemSprite != null)
            GetComponent<SpriteRenderer>().sprite = itemSprite;
    }

    public void CheckIfPickedUp()
    {
        if(WorldItemSaveData.IsPickedUp(worldItemID))
            gameObject.SetActive(false);
    }

    public bool CanInteract() => !hasBeenPickedUp && !WorldItemSaveData.IsPickedUp(worldItemID);

    public void Interact()
    {
        if(hasBeenPickedUp || WorldItemSaveData.IsPickedUp(worldItemID))
        {
            Debug.Log("Item " + worldItemID + " has already been picked up.");
            hasBeenPickedUp = true;
            return;
        }
        ItemDictionary dict = FindObjectOfType<ItemDictionary>();
        GameObject prefab = dict.GetItemPrefab(itemID);
        Debug.Log("WorldItem Interact called, itemID: " + itemID + " prefab found: " + (prefab != null));

        if(prefab != null && InventoryController.Instance.AddItem(prefab))
        {
            // Show popup
            Item item = prefab.GetComponent<Item>();
            if(item != null)
                ItemPickupUIController.Instance?.ShowItemPickup(item.Name, item.Description, GetComponent<SpriteRenderer>().sprite);

            WorldItemSaveData.MarkPickedUp(worldItemID);
            //Hide interaction icon before disabling
            InteractionDetector detector = FindObjectOfType<InteractionDetector>();
            if(detector != null)
            {
                detector.ClearInteractable();
            }
            gameObject.SetActive(false);
        }
        else
        {
            // If inventory is full, reset flag so player can try again
            hasBeenPickedUp = false;
        }
    }
}