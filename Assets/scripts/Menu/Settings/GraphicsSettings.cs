using System;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour, IResetable
{
    [SerializeField, FormerlySerializedAs("FullScreen")]
    private Toggle fullScreen;

    [SerializeField, FormerlySerializedAs("ResolutionDropdown")]
    private TMP_Dropdown resolutionDropdown;

    private List<string> resolutionList;
    private Resolution[] screenResolutions;



    public void UpdateValues()
    {
        var x = resolutionDropdown;
        var y = resolutionList;
        var z = SettingsMenu.Data;
        resolutionDropdown.SetValueWithoutNotify(Array.IndexOf(resolutionList.ToArray(),
            $"{SettingsMenu.Data.ResolutionWidth}X{SettingsMenu.Data.ResolutionHeight}"));
        fullScreen.isOn = SettingsMenu.Data.IsFullScreen;
        UpdateScreen();
    }

    private void Awake()
    {
        resolutionList = Screen.resolutions.Select(x => $"{x.width}X{x.height}").Distinct().ToList();
        var screenHZ = Screen.currentResolution.refreshRateRatio;
        screenResolutions = Screen.resolutions.Where(x=>Math.Abs(x.refreshRateRatio.value - screenHZ.value) < 1e-3).ToArray();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.SetValueWithoutNotify(Array.IndexOf(resolutionList.ToArray(),
            $"{SettingsMenu.Data.ResolutionWidth}X{SettingsMenu.Data.ResolutionHeight}"));
        fullScreen.isOn = SettingsMenu.Data.IsFullScreen;
    }

    public void UpdateScreen()
    {
        var resolution = screenResolutions[resolutionDropdown.value];
        SettingsMenu.Data.ResolutionWidth = resolution.width;
        SettingsMenu.Data.ResolutionHeight = resolution.height;
        SettingsMenu.Data.IsFullScreen = fullScreen.isOn;
        Screen.SetResolution(resolution.width, resolution.height, fullScreen.isOn);
    }
}