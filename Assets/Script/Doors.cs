using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    public bool IsLocked = true;
    public bool IsOpen = false;

    [Header("Door Objects")]
    public GameObject closedDoor;
    public GameObject openDoor;

    [Header("Physical Collider")]
    public Collider2D doorCollider;

    [Header("Key Settings")]
    public int keyID = 9;

    private void Start()
    {
        UpdateDoorVisual();
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        InventoryController inventory =
            InventoryController.Instance;

        // LOCKED DOOR
        if (IsLocked)
        {
            if (inventory != null)
            {
                var items = inventory.GetItemCounts();

                if (items.ContainsKey(keyID))
                {
                    inventory.RemoveItemsFromInventory(keyID, 1);

                    IsLocked = false;

                    Debug.Log("Door unlocked using key.");
                }
                else
                {
                    Debug.Log("Door is locked.");
                    return;
                }
            }
        }

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        IsOpen = !IsOpen;

        UpdateDoorVisual();
    }

    private void UpdateDoorVisual()
    {
        if (closedDoor != null)
        {
            closedDoor.SetActive(!IsOpen);
        }

        if (openDoor != null)
        {
            openDoor.SetActive(IsOpen);
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = !IsOpen;
        }
    }
}