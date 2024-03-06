using Assets.scripts;
using Assets.scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsData Data=SettingsCore.ReadSettings();

    //public static void SetSettingsData(SettingsData data)
    //{
    //    Data.MusicPath=data.MusicPath;
    //    Data.ResolutionWidth=data.ResolutionWidth;
    //    Data.ResolutionHeight=data.ResolutionHeight;
    //    Data.IsFullScreen=data.IsFullScreen;
    //    Data.GlobalMusicVolume=data.GlobalMusicVolume;
    //    Data.GlobalSoundVolume=data.GlobalSoundVolume;
    //    Data.StartPlayList=data.StartPlayList;
    //    Data.StartSongIndex=data.StartSongIndex;
    //}

    [SerializeField, FormerlySerializedAs("cameraAnimator")]
    private Animator cameraAnimator;

    [SerializeField, FormerlySerializedAs("Common")]
    private GameObject common;

    [SerializeField, FormerlySerializedAs("CommonSettings")]
    private CommonSettings commonSettings;

    [SerializeField, FormerlySerializedAs("ControllerManager")]
    private ControllerManager controllerManager;

    [SerializeField, FormerlySerializedAs("Graphics")]
    private GameObject graphics;

    [SerializeField, FormerlySerializedAs("GraphicsSettings")]
    private GraphicsSettings graphicsSettings;

    [SerializeField, FormerlySerializedAs("Movement")]
    private GameObject movement;

    [SerializeField, FormerlySerializedAs("Sound")]
    private GameObject sound;

    [SerializeField, FormerlySerializedAs("Sounder")]
    private AudioSource sounder;

    [SerializeField, FormerlySerializedAs("SounderController")]
    private SoundsController sounderController;

    [SerializeField, FormerlySerializedAs("SoundSettings")]
    private SoundSettings soundSettings;

    [SerializeField, FormerlySerializedAs("UnsaveWarning")]
    private GameObject unsaveWarning;

    public void EnableCommon()
    {
        movement.SetActive(false);
        graphics.SetActive(false);
        sound.SetActive(false);
        common.SetActive(true);
    }

    public void EnableGraphics()
    {
        movement.SetActive(false);
        sound.SetActive(false);
        common.SetActive(false);
        graphics.SetActive(true);
    }

    public void EnableMovement()
    {
        graphics.SetActive(false);
        sound.SetActive(false);
        common.SetActive(false);
        movement.SetActive(true);
    }

    public void EnableSound()
    {
        movement.SetActive(false);
        graphics.SetActive(false);
        common.SetActive(false);
        sound.SetActive(true);
    }

    public void SetDefault()
    {
        SettingsCore.SetSettings(SettingsCore.ReadDefaultSettings());
        MusicCore.ReadNamesOfMusic();
        UpdateSettingsValues();
    }

    public void TryQuit()
    {
        if (SettingsCore.ReadSettings() != Data)
            unsaveWarning.SetActive(true);
        else
            Quit();
    }

    public void Quit()
    {
        unsaveWarning.SetActive(false);
        SettingsCore.SetSettings(SettingsCore.ReadSettings());
        MusicCore.ReadNamesOfMusic();
        sounderController.PlaySound(sounder, sounderController.Library.FX, Sounds.Transition, 60f);
        cameraAnimator.SetBool("IsEnableSettings", false);
        controllerManager.StopClock();
        UpdateSettingsValues();
    }

    public void Cancel()
    {
        unsaveWarning.SetActive(false);
    }

    public void Save()
    {
        SettingsCore.SetSettings(Data);
        MusicCore.ReadNamesOfMusic();
        if (MusicCore.PlayListNaming.Count==0) return;
        SettingsCore.WriteSettingsTo(Data, PathCore.SettingsFilePath);
    }

    private void UpdateSettingsValues()
    {
        commonSettings.UpdateValues();
        soundSettings.UpdateValues();
        graphicsSettings.UpdateValues();
    }
}