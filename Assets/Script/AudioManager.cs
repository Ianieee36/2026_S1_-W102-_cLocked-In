using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource menuMusicSource;
    public AudioSource gameplayMusicSource;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    // =========================
    // MENU MUSIC
    // =========================

    public void PlayMenuMusic()
    {
        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.volume = 1f;
            menuMusicSource.Play();
        }
    }

    // =========================
    // GAMEPLAY MUSIC
    // =========================

    public void StartGameplayMusic()
    {
        StartCoroutine(FadeToGameplayMusic());
    }

    private IEnumerator FadeToGameplayMusic()
    {
        gameplayMusicSource.volume = 0f;
        gameplayMusicSource.Play();

        float timer = 0f;

        float startMenuVolume = menuMusicSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            menuMusicSource.volume =
                Mathf.Lerp(startMenuVolume, 0f, t);

            gameplayMusicSource.volume =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        menuMusicSource.Stop();

        menuMusicSource.volume = 1f;
    }

    public void StartMenuMusicTransition()
    {
        StartCoroutine(FadeToMenuMusic());
    }

    private IEnumerator FadeToMenuMusic()
    {
        // Start menu music silently
        menuMusicSource.volume = 0f;

        if (!menuMusicSource.isPlaying)
        {
            menuMusicSource.Play();
        }

        float timer = 0f;

        float gameplayStartVolume = gameplayMusicSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            gameplayMusicSource.volume =
                Mathf.Lerp(gameplayStartVolume, 0f, t);

            menuMusicSource.volume =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        gameplayMusicSource.Stop();

        gameplayMusicSource.volume = 1f;
        menuMusicSource.volume = 1f;
    }
}