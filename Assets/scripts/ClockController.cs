using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameObject MinuteArrow;
    public GameObject HourArrow;

    public Animator PendulumAnimator;
    public static bool IsWalking = true;
    void Start()
    {
    }

    void Update()
    {
    }

    public IEnumerator ClockWalk()
    {
        PendulumAnimator.SetBool("InSettings", true);
        while (IsWalking)
        {
            yield return new WaitForSecondsRealtime(1f);
            MinuteArrow.transform.Rotate(Vector3.forward, 360f/3600f);
            HourArrow.transform.Rotate(Vector3.forward, 360f/3600f/12f);
        }
        PendulumAnimator.SetBool("InSettings", false);
    }
}
