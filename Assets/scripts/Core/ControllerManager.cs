using UnityEngine;
using UnityEngine.Serialization;

public class ControllerManager : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("clockController")]
    private ClockController clockController;

    public void StartClock()
    {
        ClockController.IsWalking = true;
        StartCoroutine(clockController.ClockWalk());
    }

    public void StopClock()
    {
        ClockController.IsWalking = false;
    }
}