using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0.6f));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0.6f));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 1.0f));
    

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.resolutions[i].height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetQuality(0);

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume (float masterVolume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }
    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", musicVolume);

    }
    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sfxVolume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
