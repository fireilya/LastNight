using System.Collections;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public static bool IsWalking = true;
    public GameObject HourArrow;
    public GameObject MinuteArrow;

    public Animator PendulumAnimator;

    public IEnumerator ClockWalk()
    {
        PendulumAnimator.SetBool("InSettings", true);
        while (IsWalking)
        {
            yield return new WaitForSecondsRealtime(1f);
            MinuteArrow.transform.Rotate(Vector3.forward, 360f / 3600f);
            HourArrow.transform.Rotate(Vector3.forward, 360f / 3600f / 12f);
        }

        PendulumAnimator.SetBool("InSettings", false);
    }
}