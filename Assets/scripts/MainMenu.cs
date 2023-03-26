using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    public Animator CameraAnimator;
    public SoundsController SounderController;
    public AudioSource Sounder;
    public ControllerManager ControllerManager;
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
