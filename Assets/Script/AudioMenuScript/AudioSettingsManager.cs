using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioSettingsManager : MonoBehaviour
{
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeMasterVolume()
    {
        if (mainAudioMixer == null || masterVol == null) return; 
        mainAudioMixer.SetFloat("Master", Mathf.Log10(masterVol.value) * 20);
    }

    public void ChangeMusicVolume()
    {
        if (mainAudioMixer == null || musicVol == null) return;
        mainAudioMixer.SetFloat("Music", Mathf.Log10(musicVol.value) * 20);
    }

    public void ChangeSFXVolume()
    {
        if (mainAudioMixer == null || sfxVol == null) return;
        mainAudioMixer.SetFloat("SFX", Mathf.Log10(sfxVol.value) * 20);
    }


    
}
