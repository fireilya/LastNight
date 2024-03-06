using Assets.scripts;
using Assets.scripts.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("CameraAnimator")]
    private Animator cameraAnimator;

    [SerializeField, FormerlySerializedAs("ControllerManager")]
    private ControllerManager controllerManager;

    [SerializeField, FormerlySerializedAs("Sounder")]
    private AudioSource sounder;

    [SerializeField, FormerlySerializedAs("SounderController")]
    private SoundsController sounderController;

    public void EnableSettings()
    {
        sounderController.PlaySound(sounder, sounderController.Library.FX, Sounds.Transition, 60f);
        cameraAnimator.SetBool("IsEnableSettings", true);
        controllerManager.StartClock();
    }

    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}