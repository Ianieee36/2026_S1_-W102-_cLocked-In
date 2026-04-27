using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToAccessibilitySettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadAccessibilitySettings()
    {
        SceneManager.LoadScene("AccessibilityMenu");
    }
}
