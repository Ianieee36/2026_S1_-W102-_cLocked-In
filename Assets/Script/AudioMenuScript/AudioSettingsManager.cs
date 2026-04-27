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
        mainAudioMixer.SetFloat("Master", masterVol.value);
    }

    public void ChangeMusicVolume()
    {
        if (mainAudioMixer == null || musicVol == null) return;
        mainAudioMixer.SetFloat("Music", musicVol.value);
    }

    public void ChangeSFXVolume()
    {
        if (mainAudioMixer == null || sfxVol == null) return;
        mainAudioMixer.SetFloat("SFX", sfxVol.value);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
