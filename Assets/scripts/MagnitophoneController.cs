using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;


public class MagnitophoneController : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] rotatable;

    public Animator StopButtonAnimator;
    public Animator PlayButtonAnimator;
    public Animator MoveForwardButtonAnimator;
    public Animator MoveBackButtonAnimator;

    public AudioSource Music;
    public AudioSource Sounder;
    public SoundsController SounderControl;

    public Camera MainCamera;

    private RaycastHit hit;

    public float rollSpeed = 0.0f;
    private const float playSpeed = 1.6f;
    private const float moveSpeed = 20f;
    private const float stopSpeed = 0.0f;
    private const float commonWait = 0.5f;
    private const float moveWait = 2.8f;
    private bool isPaused = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        var RayCaster = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RayCaster, out hit))
            switch (hit.collider.gameObject.name)
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
        foreach (var obj in rotatable)
        {
            obj.transform.Rotate(-Vector3.up, rollSpeed);
        }

        if (!Music.isPlaying && PlayButtonAnimator.GetBool("IsBump") && MusicCore.IsStarted)
        {
            MusicCore.MoveMusic(true, true, Music);
        }
    }

    public IEnumerator PressMoveButton(Animator buttonAnimator, bool isForward)
    {
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        var temp=rollSpeed;
        rollSpeed=moveSpeed*(isForward?1:-1);
        Music.Stop();
        MusicCore.IsStarted = false;
        isPaused=false;
        SounderControl.PlaySound(Sounder, SounderControl.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(commonWait);
        SounderControl.PlaySound(Sounder, SounderControl.FX, Sounds.MoveMusic, 120f);
        yield return new WaitForSeconds(moveWait);
        MusicCore.MoveMusic(isForward, PlayButtonAnimator.GetBool("IsBump"), Music);
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        rollSpeed = temp;
    }

    public IEnumerator PressPlayButton()
    {
        PlayButtonAnimator.SetBool("IsBump", !PlayButtonAnimator.GetBool("IsBump"));
        if (PlayButtonAnimator.GetBool("IsBump"))
        {
            if (isPaused)
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
            isPaused = true;
        }
        SounderControl.PlaySound(Sounder, SounderControl.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(commonWait);
        rollSpeed = Math.Abs(rollSpeed - stopSpeed) < 1e-3 ? playSpeed : stopSpeed;
    }

    public IEnumerator PressStopButton()
    {
        StopButtonAnimator.SetBool("IsBump", !StopButtonAnimator.GetBool("IsBump"));
        ReturnPlayButton();
        rollSpeed = stopSpeed;
        MusicCore.StopMusic(Music);
        isPaused = false;
        SounderControl.PlaySound(Sounder, SounderControl.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(commonWait);
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