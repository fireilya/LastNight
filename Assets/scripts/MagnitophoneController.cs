using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;


public class MagnitophoneController : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] Rotatable;

    public Animator StopButtonAnimator;
    public Animator PlayButtonAnimator;
    public Animator MoveForwardButtonAnimator;
    public Animator MoveBackButtonAnimator;

    public AudioSource Music;
    public AudioSource Sounder;
    public SoundsController SounderControl;

    public Camera MainCamera;

    private RaycastHit _hit;

    public float RollSpeed = 0.0f;
    private const float PlaySpeed = 1.6f;
    private const float MoveSpeed = 20f;
    private const float StopSpeed = 0.0f;
    private const float CommonWait = 0.5f;
    private const float MoveWait = 2.8f;
    private bool _isPaused = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        var rayCaster = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayCaster, out _hit))
            switch (_hit.collider.gameObject.name)
            {
                case "PlayButton":
                    StartCoroutine(PressPlayButton());
                    break;

                case "StopButton":
                    StartCoroutine(PressStopButton());
                    break;

                case "MoveForwardButton":
                    StartCoroutine(PressMoveButton(MoveForwardButtonAnimator, true));
                    break;

                case "MoveBackButton":
                    StartCoroutine(PressMoveButton(MoveBackButtonAnimator, false));
                    break;
            }
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var obj in Rotatable)
        {
            obj.transform.Rotate(-Vector3.up, RollSpeed);
        }

        if (!Music.isPlaying && PlayButtonAnimator.GetBool("IsBump") && MusicCore.IsStarted)
        {
            MusicCore.MoveMusic(true, true, Music);
        }
    }

    public IEnumerator PressMoveButton(Animator buttonAnimator, bool isForward)
    {
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        var temp=RollSpeed;
        RollSpeed=MoveSpeed*(isForward?1:-1);
        Music.Stop();
        MusicCore.IsStarted = false;
        _isPaused=false;
        SounderControl.PlaySound(Sounder, SounderControl.Fx, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        SounderControl.PlaySound(Sounder, SounderControl.Fx, Sounds.MoveMusic, 120f);
        yield return new WaitForSeconds(MoveWait);
        MusicCore.MoveMusic(isForward, PlayButtonAnimator.GetBool("IsBump"), Music);
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        RollSpeed = temp;
    }

    public IEnumerator PressPlayButton()
    {
        PlayButtonAnimator.SetBool("IsBump", !PlayButtonAnimator.GetBool("IsBump"));
        if (PlayButtonAnimator.GetBool("IsBump"))
        {
            if (_isPaused)
            {
                Music.UnPause();
            }
            else
            {
                MusicCore.PlayMusic(Music);
            }
        }
        else
        {
            Music.Pause();
            _isPaused = true;
        }
        SounderControl.PlaySound(Sounder, SounderControl.Fx, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        RollSpeed = Math.Abs(RollSpeed - StopSpeed) < 1e-3 ? PlaySpeed : StopSpeed;
    }

    public IEnumerator PressStopButton()
    {
        StopButtonAnimator.SetBool("IsBump", !StopButtonAnimator.GetBool("IsBump"));
        ReturnPlayButton();
        RollSpeed = StopSpeed;
        MusicCore.StopMusic(Music);
        _isPaused = false;
        SounderControl.PlaySound(Sounder, SounderControl.Fx, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        StopButtonAnimator.SetBool("IsBump", !StopButtonAnimator.GetBool("IsBump"));
    }


    private void ReturnPlayButton()
    {
        if (PlayButtonAnimator.GetBool("IsBump"))
        {
            PlayButtonAnimator.SetBool("IsBump", false);
        }
    }


}