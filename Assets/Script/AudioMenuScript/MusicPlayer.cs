using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance; // Static instance to ensure only one MusicPlayer exists

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate music player if another instance already exists
        }
    }
}
