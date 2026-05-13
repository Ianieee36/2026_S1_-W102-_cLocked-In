using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToAudioSettings : MonoBehaviour
{
    
    public void LoadAudioSettings()
    {
        SceneManager.LoadScene("AudioMenu"); // Load AudioMenu scene
    }
}
