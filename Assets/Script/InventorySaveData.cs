using UnityEngine;

[System.Serializable]
public class InventorySaveData
{
    public int itemID;
    public int slotIndex; //the index of the slot where the item is placed
    public int quantity = 1;
}
