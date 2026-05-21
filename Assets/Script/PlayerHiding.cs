using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public static PlayerHiding Instance;
    private bool isHiding = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hide()
    {
        isHiding = true;
        spriteRenderer.enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stop current movement
        GetComponent<PlayerMovement>().enabled = false; // Lock movement
    }

    public void Unhide()
    {
        isHiding = false;
        spriteRenderer.enabled = true;
        GetComponent<PlayerMovement>().enabled = true; // Unlock movement
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}