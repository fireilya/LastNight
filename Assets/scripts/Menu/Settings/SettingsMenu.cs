using Assets.scripts;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsData data = SettingsCore.ReadSettings();
    public Animator CameraAnimator;
    public GameObject Common;
    public CommonSettings CommonSettings;
    public ControllerManager ControllerManager;
    public GameObject Graphics;
    public GraphicsSettings GraphicsSettings;
    public GameObject Menu;
    public GameObject Movement;
    public GameObject Settings;
    public GameObject Sound;
    public AudioSource Sounder;
    public SoundsController SounderController;
    public SoundSettings SoundSettings;
    public GameObject UnsaveWarning;

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
        UpdateSettingsValues();
    }

    public void TryQuit()
    {
        if (SettingsCore.ReadSettings() != data)
            UnsaveWarning.SetActive(true);
        else
            Quit();
    }

    public void Quit()
    {
        UnsaveWarning.SetActive(false);
        SettingsCore.SetSettings(SettingsCore.ReadSettings());
        SounderController.PlaySound(Sounder, SounderController.FX, Sounds.Transition, 60f);
        CameraAnimator.SetBool("IsEnableSettings", false);
        ControllerManager.StopClock();
        UpdateSettingsValues();
    }

    public void Cancel()
    {
        UnsaveWarning.SetActive(false);
    }

    public void Save()
    {
        SettingsCore.WriteSettingsTo(data, PathCore.SettingsFilePath);
        SettingsCore.SetSettings(SettingsCore.ReadSettings());
    }

    private void UpdateSettingsValues()
    {
        CommonSettings.UpdateValues();
        SoundSettings.UpdateValues();
        GraphicsSettings.UpdateValues();
    }
}