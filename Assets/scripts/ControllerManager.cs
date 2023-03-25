using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public ClockController ClockController;

    public void StartClock()
    {
        ClockController.IsWalking=true;
        StartCoroutine(ClockController.ClockWalk());
    }

    public void StopClock()
    {
        ClockController.IsWalking = false;
    }
}

