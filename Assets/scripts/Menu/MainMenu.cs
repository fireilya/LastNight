using Assets.scripts;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator CameraAnimator;
    public ControllerManager ControllerManager;
    public AudioSource Sounder;
    public SoundsController SounderController;

    public void EnableSettings()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, Sounds.Transition, 60f);
        CameraAnimator.SetBool("IsEnableSettings", true);
        ControllerManager.StartClock();
    }


    public void QuitApplication()
    {
        Application.Quit();
    }
}