using Assets.scripts.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour, IResetable
{
    [SerializeField, FormerlySerializedAs("MusicVolume")]
    private Slider musicVolume;

    [SerializeField, FormerlySerializedAs("SoundVolume")]
    private Slider soundVolume;

    public void UpdateValues()
    {
        musicVolume.value = MixerController.MusicVolume;
        soundVolume.value = MixerController.SoundVolume;
    }

    private void Start()
    {
        musicVolume.value = MixerController.MusicVolume;
        soundVolume.value = MixerController.SoundVolume;
    }

    public void SetMusicVolume()
    {
        MixerController.MusicVolume = musicVolume.value;
        SettingsMenu.Data.GlobalMusicVolume = musicVolume.value;
    }

    public void SetSoundsVolume()
    {
        MixerController.SoundVolume = soundVolume.value;
        SettingsMenu.Data.GlobalSoundVolume = soundVolume.value;
    }
}