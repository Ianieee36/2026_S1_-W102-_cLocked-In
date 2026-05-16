using UnityEngine;
using UnityEngine.UI;

public class CoffeeItem : Item
{
    public GameObject emptyCupPrefab; // Drag empty cup prefab here
    public float boostDuration = 10f;
    public float boostedSprintSpeed = 15f;

    private bool isConsumed = false;

    public override void UseItem()
    {
        if (isConsumed) return;
        isConsumed = true;

        Debug.Log("Coffee consumed");

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
            player.ApplySprintBoost(boostedSprintSpeed, boostDuration);

        Slot slot = GetComponentInParent<Slot>();
        Debug.Log("Slot found: " + (slot != null));
        Debug.Log("Empty cup prefab: " + (emptyCupPrefab != null));

        if (slot != null && emptyCupPrefab != null)
        {
            Destroy(gameObject);
            GameObject emptyCup = Instantiate(emptyCupPrefab, slot.transform);
            emptyCup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            slot.currentItem = emptyCup;
            Debug.Log("Empty cup spawned: " + emptyCup.name);
        }
    }
}