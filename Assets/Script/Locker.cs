using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    public Sprite closeSprite;
    public Sprite farSprite;
    private SpriteRenderer spriteRenderer;
    private bool isOccupied = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = farSprite;
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

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        PlayerHiding playerHiding = PlayerHiding.Instance;
        if (playerHiding == null) return;

        if (!isOccupied)
        {
            isOccupied = true;
            playerHiding.Hide();
            Debug.Log("Player hiding in locker");
        }
        else
        {
            isOccupied = false;
            playerHiding.Unhide();
            Debug.Log("Player left locker");
        }
    }
}