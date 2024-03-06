using Assets.scripts;
using Assets.scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationSync : MonoBehaviour
{
    private readonly Sounds[] _tikTak =
    {
        Sounds.EnterTik,
        Sounds.OutTik
    };
        
    private bool isMenuEnabled = true;
    private bool isSettingsEnabled;

    [SerializeField, FormerlySerializedAs("Menu")]
    private GameObject menu;

    [SerializeField, FormerlySerializedAs("Settings")]
    private GameObject settings;

    [SerializeField, FormerlySerializedAs("Sounder")]
    private AudioSource sounder;

    [SerializeField, FormerlySerializedAs("SounderController")]
    private SoundsController sounderController;
    private byte tik;

    public void PlayTik()
    {
        sounderController.PlaySound(sounder, sounderController.Library.FX, _tikTak[++tik % 2]);
    }

    public void ToggleSetting()
    {
        isSettingsEnabled = !isSettingsEnabled;
        settings.SetActive(isSettingsEnabled);
    }

    public void ToggleMenu()
    {
        isMenuEnabled = !isMenuEnabled;
        menu.SetActive(isMenuEnabled);
    }
}