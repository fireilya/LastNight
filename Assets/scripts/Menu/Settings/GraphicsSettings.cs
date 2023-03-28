using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour, IResetable
{
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullScreen;
    private List<string> resolutionList;
    private Resolution[] screenResolutions;
    void Start()
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
        SettingsMenu.data.IsFullScreen=FullScreen.isOn;
        Screen.SetResolution(resolution.width, resolution.height, FullScreen.isOn);
    }

    public void UpdateValues()
    {
        ResolutionDropdown.SetValueWithoutNotify(Array.IndexOf(resolutionList.ToArray(),
            $"{SettingsMenu.data.ResolutionWidth} X {SettingsMenu.data.ResolutionHeight}"));
        FullScreen.isOn = SettingsMenu.data.IsFullScreen;
        UpdateScreen();
    }
}
