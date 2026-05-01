using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeMasterVolume() // Call this method when the master volume slider value changes
    {
        if (mainAudioMixer == null || masterVol == null) return; 
        mainAudioMixer.SetFloat("Master", Mathf.Log10(masterVol.value) * 20);
    }

    public void ChangeMusicVolume() // Call this method when the music volume slider value changes
    {
        if (mainAudioMixer == null || musicVol == null) return;
        mainAudioMixer.SetFloat("Music", Mathf.Log10(musicVol.value) * 20);
    }

    public void ChangeSFXVolume() // Call this method when the SFX volume slider value changes
    {
        if (mainAudioMixer == null || sfxVol == null) return; 
        mainAudioMixer.SetFloat("SFX", Mathf.Log10(sfxVol.value) * 20); // Convert slider value to decibels and set it in the audio mixer
    }


    
}
