using UnityEngine;

public class PaperItem : Item
{
    public GameObject paperAirplanePrefab; // Drag paper airplane prefab here
    private bool isTransformed = false;

    public override void UseItem()
    {
        if (isTransformed) return;
        isTransformed = true;

        Slot slot = GetComponentInParent<Slot>();
        Debug.Log("Slot found: " + (slot != null));

        if (slot != null && paperAirplanePrefab != null)
        {
            Destroy(gameObject);
            GameObject airplane = Instantiate(paperAirplanePrefab, slot.transform);
            airplane.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            slot.currentItem = airplane;
            Debug.Log("Paper transformed into airplane");
        }
    }
}