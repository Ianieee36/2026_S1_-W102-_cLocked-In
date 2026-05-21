using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10; //1-0 on the keyboard

    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        hotbarKeys = new Key[slotCount];
        for(int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check for key presses
        for(int i = 0; i < slotCount; i++)
        {
            if(Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                //UseItem
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySaveData { 
                    itemID = item.ID, 
                    slotIndex = slotTransform.GetSiblingIndex(), 
                    quantity = item.quantity
                });
            }
        }
        return hotbarData;
    }

    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        // Collect children into a list first before destroying
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in hotbarPanel.transform)
        {
            children.Add(child.gameObject);
        }
        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }

        // Create new slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    Item itemComponent  = item.GetComponent<Item>();
                    if(itemComponent != null)
                    {
                        itemComponent.quantity = data.quantity;
                        itemComponent.UpdateQuantityDisplay();
                    }
                    
                    slot.currentItem = item;
                }
            }
        }
    }
}
