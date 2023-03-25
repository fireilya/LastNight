using Assets.scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SettingsMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    public Animator CameraAnimator;
    public SoundsController SounderController;
    public AudioSource Sounder;
    public ControllerManager ControllerManager;
    public void EnableCommon()
    {

    }

    public void EnableGraphics()
    {

    }

    public void EnableMovement()
    {

    }

    public void EnableSound()
    {
    }
    public void SetDefault()
    {
    }

    public void Quit()
    {
        SounderController.PlaySound(Sounder, SounderController.FX, Sounds.Transition, 60f);
        CameraAnimator.SetBool("IsEnableSettings", false);
        ControllerManager.StopClock();
    }


    public void Save()
    {

    }
}
