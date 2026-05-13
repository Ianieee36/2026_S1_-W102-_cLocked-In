using UnityEngine;

public class ComputerController : MonoBehaviour, IInteractable
{
   public GameObject screenUI; // Drag your UI panel here
    public Sprite closeSprite;
    public Sprite farSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = farSprite;
        screenUI.SetActive(false);
    }

    public bool CanInteract()
    {
        return true; // Computer can always be interacted with
    }

    public void Interact()
    {
        isOpen = !isOpen;
        screenUI.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f; // Pause game while screen is open
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            spriteRenderer.sprite = closeSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            spriteRenderer.sprite = farSprite;
    } 
}
