using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    private void Start()
    {
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        masterVol.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVol.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVol.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSFXVolume();
    }

    public void ChangeMasterVolume()
    {
        if (mainAudioMixer == null || masterVol == null) return;

        float value = Mathf.Max(masterVol.value, 0.0001f);
        mainAudioMixer.SetFloat("Master", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("MasterVolume", masterVol.value);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume()
    {
        if (mainAudioMixer == null || musicVol == null) return;

        float value = Mathf.Max(musicVol.value, 0.0001f);
        mainAudioMixer.SetFloat("Music", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("MusicVolume", musicVol.value);
        PlayerPrefs.Save();
    }

    public void ChangeSFXVolume()
    {
        if (mainAudioMixer == null || sfxVol == null) return;

        float value = Mathf.Max(sfxVol.value, 0.0001f);
        mainAudioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("SFXVolume", sfxVol.value);
        PlayerPrefs.Save();
    }
}