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
    private bool _isSettingsEnabled;
    private bool _isMenuEnabled=true;
    private byte _tik = 0;

    private Sounds[] _tikTak = new Sounds[]
    {
        Sounds.EnterTik,
        Sounds.OutTik
    };

    public void PlayTik()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, _tikTak[++_tik%2]);
    }

    public void ToggleSetting()
    {
        _isSettingsEnabled = !_isSettingsEnabled;
        Settings.SetActive(_isSettingsEnabled);
    }

    public void ToggleMenu()
    {
        _isMenuEnabled = !_isMenuEnabled;
        Menu.SetActive(_isMenuEnabled);
    }
}
