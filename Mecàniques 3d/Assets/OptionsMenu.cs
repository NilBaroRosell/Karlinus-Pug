using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public bool silenced;
    public float lastVolume;

    Resolution[] resolutions;

    private void Start()
    {
        silenced = false;
        lastVolume = 0;
        audioMixer.SetFloat("Volume", 0);

        resolutions = new Resolution[3];
        resolutions[0].width = 1920;
        resolutions[0].height = 1080;
        resolutions[1].width = 1280;
        resolutions[1].height = 720;
        resolutions[2].width = 852;
        resolutions[2].height = 480;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    private void Update()
    {
        
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Debug.Log("HELLO");
    }

    public void SetVolume(float volume)
    {
        if(!silenced)
        {
            audioMixer.SetFloat("Volume", volume);
        }
        lastVolume = volume;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetMusic(bool isOn)
    {
        silenced = !isOn;
        if(silenced) audioMixer.SetFloat("Volume", -80);
        else audioMixer.SetFloat("Volume", lastVolume);
    }
}
