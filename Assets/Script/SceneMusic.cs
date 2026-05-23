using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public enum MusicType
    {
        MainMenu,
        Gameplay
    }

    public MusicType musicType;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        if (musicType == MusicType.MainMenu)
        {
            AudioManager.Instance.PlayMenuMusic();
        }
        else if (musicType == MusicType.Gameplay)
        {
            AudioManager.Instance.StartGameplayMusic();
        }
    }
}