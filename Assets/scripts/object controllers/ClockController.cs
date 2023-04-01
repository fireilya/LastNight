using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ClockController : MonoBehaviour
{
    public static bool IsWalking = true;
    [SerializeField, FormerlySerializedAs("hourArrow")]
    private GameObject hourArrow;
    [SerializeField, FormerlySerializedAs("minuteArrow")]
    private GameObject minuteArrow;
    [SerializeField] 
    private Animator pendulumAnimator;

    public IEnumerator ClockWalk()
    {
        pendulumAnimator.SetBool("InSettings", true);
        while (IsWalking)
        {
            yield return new WaitForSecondsRealtime(1f);
            minuteArrow.transform.Rotate(Vector3.forward, 360f / 3600f);
            hourArrow.transform.Rotate(Vector3.forward, 360f / 3600f / 12f);
        }

        pendulumAnimator.SetBool("InSettings", false);
    }
}