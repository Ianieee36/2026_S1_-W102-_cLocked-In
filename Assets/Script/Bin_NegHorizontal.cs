using UnityEngine;

public class Bin_NegHorizontal : MonoBehaviour, IInteractable
{
    public bool IsUsed { get; private set; }
    public string PrinterID { get; private set; }
    public GameObject itemPrefab; //Item that chest drops

    public Sprite closeSprite;
    public Sprite farSprite;
    private SpriteRenderer spriteRenderer;

    public bool CanInteract()
    {
        return !IsUsed;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        UsePrinter();
        //Used Printer already
    }

    private void UsePrinter()
    {
        Debug.Log("UsePrinter called");
        SetUsed(true);
        if (itemPrefab != null)
        {
            Debug.Log("Spawning item");
            Instantiate(itemPrefab, new Vector3(transform.position.x - 1f, transform.position.y), Quaternion.identity);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PrinterID ??= GlobalHelper.GenerateUniqueID(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = farSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = closeSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = farSprite;
        }
    }

    public void SetUsed(bool used)
    {
        IsUsed = used;
    }
}
