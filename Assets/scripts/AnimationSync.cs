using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.scripts;
using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    public AudioSource Sounder;
    public SoundsController SounderController;
    public GameObject Settings;
    public GameObject Menu;
    private bool IsSettingsEnabled;
    private bool IsMenuEnabled=true;
    private byte Tik = 0;

    private Sounds[] TikTak = new Sounds[]
    {
        Sounds.EnterTik,
        Sounds.OutTik
    };

    public void PlayTik()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, TikTak[++Tik%2]);
    }

    public void ToggleSetting()
    {
        IsSettingsEnabled = !IsSettingsEnabled;
        Settings.SetActive(IsSettingsEnabled);
    }

    public void ToggleMenu()
    {
        IsMenuEnabled = !IsMenuEnabled;
        Menu.SetActive(IsMenuEnabled);
    }
}
