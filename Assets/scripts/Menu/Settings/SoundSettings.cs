using Assets.scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour, IResetable
{
    public Slider MusicVolume;
    public Slider SoundVolume;

    public void UpdateValues()
    {
        MusicVolume.value = MixerController.MusicVolume;
        SoundVolume.value = MixerController.SoundVolume;
    }

    private void Start()
    {
        MusicVolume.value = MixerController.MusicVolume;
        SoundVolume.value = MixerController.SoundVolume;
    }

    public void SetMusicVolume()
    {
        MixerController.MusicVolume = MusicVolume.value;
        SettingsMenu.data.GlobalMusicVolume = MusicVolume.value;
    }

    public void SetSoundsVolume()
    {
        MixerController.SoundVolume = SoundVolume.value;
        SettingsMenu.data.GlobalSoundVolume = SoundVolume.value;
    }
}