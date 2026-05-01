using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToAccessibilitySettings : MonoBehaviour
{
    
    public void LoadAccessibilitySettings()
    {
        SceneManager.LoadScene("AccessibilityMenu"); // Load AccessibilityMenu scene 
    }
}
