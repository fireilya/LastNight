using Assets.scripts;
using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    private readonly Sounds[] _tikTak =
    {
        Sounds.EnterTik,
        Sounds.OutTik
    };

    public GameObject Common;
    private bool isMenuEnabled = true;
    private bool isSettingsEnabled;
    public GameObject Menu;
    public GameObject Settings;
    public AudioSource Sounder;
    public SoundsController SounderController;
    private byte tik;

    public void PlayTik()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, _tikTak[++tik % 2]);
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