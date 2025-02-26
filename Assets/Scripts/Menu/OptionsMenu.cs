using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // Initialize sliders with saved values
        InitializeSliders();

        // Add listeners to sliders
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    public void SetSFXVolume()
    {
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }
    public void SetMusicVolume()
    {
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void InitializeSliders()
    {
        // Get saved values (or defaults if not saved)
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
    }
}
