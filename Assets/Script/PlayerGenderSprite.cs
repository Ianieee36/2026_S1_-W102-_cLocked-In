using UnityEngine;

public class PlayerGenderSprite : MonoBehaviour
{
    public Sprite boySprite;
    public Sprite girlSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplySavedGender();
    }

    public void ApplySavedGender()
    {
        string savedGender = PlayerPrefs.GetString("PlayerGender", "Boy");

        if (savedGender == "Girl")
        {
            spriteRenderer.sprite = girlSprite;
        }
        else
        {
            spriteRenderer.sprite = boySprite;
        }
    }
}