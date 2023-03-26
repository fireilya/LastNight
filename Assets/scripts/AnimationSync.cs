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
    public GameObject Common;
    private bool isSettingsEnabled;
    private bool isMenuEnabled=true;
    private byte tik;

    private Sounds[] _tikTak = new Sounds[]
    {
        Sounds.EnterTik,
        Sounds.OutTik
    };

    public void PlayTik()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, _tikTak[++tik%2]);
    }

    public void ToggleSetting()
    {
        isSettingsEnabled = !isSettingsEnabled;
        Settings.SetActive(isSettingsEnabled);
    }

    public void ToggleMenu()
    {
        isMenuEnabled = !isMenuEnabled;
        Menu.SetActive(isMenuEnabled);
    }
}
