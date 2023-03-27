using System;
using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SettingsMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    public GameObject Common;
    public GameObject Movement;
    public GameObject Graphics;
    public GameObject Sound;
    public Animator CameraAnimator;
    public SoundsController SounderController;
    public AudioSource Sounder;
    public ControllerManager ControllerManager;
    public static SettingsData data = SettingsCore.ReadSettings();
    public CommonSettings CommonSettings;
    public SoundSettings SoundSettings;

    public void EnableCommon()
    {
        Movement.SetActive(false);
        Graphics.SetActive(false);
        Sound.SetActive(false);
        Common.SetActive(true);
    }

    public void EnableGraphics()
    {
        Movement.SetActive(false);
        Sound.SetActive(false);
        Common.SetActive(false);
        Graphics.SetActive(true);
    }

    public void EnableMovement()
    {
        Graphics.SetActive(false);
        Sound.SetActive(false);
        Common.SetActive(false);
        Movement.SetActive(true);
    }

    public void EnableSound()
    {
        Movement.SetActive(false);
        Graphics.SetActive(false);
        Common.SetActive(false);
        Sound.SetActive(true);
    }
    public void SetDefault()
    {
        SettingsCore.SetSettings(SettingsCore.ReadDefaultSettings());
        CommonSettings.UpdateValues();
        SoundSettings.UpdateValues();
    }

    public void Quit()
    {
        if (SettingsCore.ReadSettings() != data)
        {
            Debug.Log("Looser!");
        }
        SounderController.PlaySound(Sounder, SounderController.FX, Sounds.Transition, 60f);
        CameraAnimator.SetBool("IsEnableSettings", false);
        ControllerManager.StopClock();
    }


    public void Save()
    {
        SettingsCore.WriteSettingsTo(data, PathCore.SettingsFilePath);
        SettingsCore.SetSettings(SettingsCore.ReadSettings());
    }
}
