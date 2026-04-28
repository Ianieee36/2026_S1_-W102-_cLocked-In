using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            bool isOpen = !menuCanvas.activeSelf;
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            Time.timeScale = isOpen ? 0f : 1f;
        }
    }
}
