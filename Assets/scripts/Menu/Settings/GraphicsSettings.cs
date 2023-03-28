using System;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour, IResetable
{
    public Toggle FullScreen;
    public TMP_Dropdown ResolutionDropdown;
    private List<string> resolutionList;
    private Resolution[] screenResolutions;

    public void UpdateValues()
    {
        ResolutionDropdown.SetValueWithoutNotify(Array.IndexOf(resolutionList.ToArray(),
            $"{SettingsMenu.data.ResolutionWidth} X {SettingsMenu.data.ResolutionHeight}"));
        FullScreen.isOn = SettingsMenu.data.IsFullScreen;
        UpdateScreen();
    }

    private void Start()
    {
        resolutionList = Screen.resolutions.Select(x => $"{x.width}X{x.height}").ToList();
        screenResolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(resolutionList);
        ResolutionDropdown.SetValueWithoutNotify(Array.IndexOf(resolutionList.ToArray(),
            $"{SettingsMenu.data.ResolutionWidth}X{SettingsMenu.data.ResolutionHeight}"));
        FullScreen.isOn = SettingsMenu.data.IsFullScreen;
    }

    public void UpdateScreen()
    {
        var resolution = screenResolutions[ResolutionDropdown.value];
        SettingsMenu.data.ResolutionWidth = resolution.width;
        SettingsMenu.data.ResolutionHeight = resolution.height;
        SettingsMenu.data.IsFullScreen = FullScreen.isOn;
        Screen.SetResolution(resolution.width, resolution.height, FullScreen.isOn);
    }
}