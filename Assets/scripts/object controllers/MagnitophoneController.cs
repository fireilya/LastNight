using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Assets.scripts;
using Assets.scripts.Enum;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class MagnitophoneController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, FormerlySerializedAs("rotatable")] 
    private GameObject[] rotatable;
    [SerializeField, FormerlySerializedAs("stopButtonAnimator")] 
    private Animator stopButtonAnimator;

    [SerializeField, FormerlySerializedAs("playButtonAnimator")] 
    private Animator playButtonAnimator;

    [SerializeField, FormerlySerializedAs("moveForwardButtonAnimator")] 
    private Animator moveForwardButtonAnimator;

    [SerializeField, FormerlySerializedAs("moveBackButtonAnimator")] 
    private Animator moveBackButtonAnimator;

    [SerializeField, FormerlySerializedAs("music")] 
    private AudioSource music;

    [SerializeField, FormerlySerializedAs("sounder")]
    private AudioSource sounder;

    [SerializeField, FormerlySerializedAs("sounderControl")] 
    private SoundsController sounderControl;

    [SerializeField, FormerlySerializedAs("mainCamera")]
    private Camera mainCamera;

    private RaycastHit _hit;
    public AudioSourceData musicSourceData;
    private float RollSpeed = 0.0f;
    private const float PlaySpeed = 100.6f;
    private const float MoveSpeed = 2000f;
    private const float StopSpeed = 0.0f;
    private const float CommonWait = 0.5f;
    private const float MoveWait = 2.8f;
    public bool IsPaused { get; private set; }
    private static bool IsButtonsReady;
    public void OnPointerClick(PointerEventData eventData)
    {
        var rayCaster = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(rayCaster, out _hit) || !IsButtonsReady) return;
        IsButtonsReady = false;
        switch (_hit.collider.gameObject.name)
        {
            case "PlayButton":
                StartCoroutine(PressPlayButton());
                break;

            case "StopButton":
                StartCoroutine(PressStopButton());
                break;

            case "MoveForwardButton":
                StartCoroutine(PressMoveButton(moveForwardButtonAnimator, true));
                break;

            case "MoveBackButton":
                StartCoroutine(PressMoveButton(moveBackButtonAnimator, false));
                break;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        musicSourceData = new AudioSourceData(music);
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var obj in rotatable)
        {
            obj.transform.Rotate(-Vector3.up, RollSpeed*Time.deltaTime);
        }

        if (!musicSourceData.Source.isPlaying && !IsPaused && !musicSourceData.IsStoped && musicSourceData.IsStarted && MusicCore.IsReady)
        {
            MusicCore.MoveMusic(true, true, musicSourceData);
        }
    }

    public IEnumerator PressMoveButton(Animator buttonAnimator, bool isForward)
    {
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        var temp=RollSpeed;
        RollSpeed=MoveSpeed*(isForward?1:-1);
        musicSourceData.IsStoped = true;
        music.Stop();
        IsPaused =false;
        sounderControl.PlaySound(sounder, sounderControl.Library.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        sounderControl.PlaySound(sounder, sounderControl.Library.FX, Sounds.MoveMusic, 120f);
        yield return new WaitForSeconds(MoveWait);
        MusicCore.MoveMusic(isForward, playButtonAnimator.GetBool("IsBump"), musicSourceData);
        buttonAnimator.SetBool("IsBump", !buttonAnimator.GetBool("IsBump"));
        RollSpeed = temp;
        IsButtonsReady = true;
    }

    public IEnumerator PressPlayButton()
    {
        playButtonAnimator.SetBool("IsBump", !playButtonAnimator.GetBool("IsBump"));
        if (playButtonAnimator.GetBool("IsBump"))
        {
            if (IsPaused)
            {
                music.UnPause();
                IsPaused=false;
            }
            else
            {
                MusicCore.PlayMusic(musicSourceData);
            }
        }
        else
        {
            music.Pause();
            IsPaused = true;
        }
        sounderControl.PlaySound(sounder, sounderControl.Library.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        RollSpeed = Math.Abs(RollSpeed - StopSpeed) < 1e-3 ? PlaySpeed : StopSpeed;
        IsButtonsReady=true;
    }

    public IEnumerator PressStopButton()
    {
        stopButtonAnimator.SetBool("IsBump", !stopButtonAnimator.GetBool("IsBump"));
        ReturnPlayButton();
        RollSpeed = StopSpeed;
        MusicCore.StopMusic(musicSourceData);
        IsPaused = false;
        sounderControl.PlaySound(sounder, sounderControl.Library.FX, Sounds.PressButtonLatch);
        yield return new WaitForSeconds(CommonWait);
        stopButtonAnimator.SetBool("IsBump", !stopButtonAnimator.GetBool("IsBump"));
        IsButtonsReady = true;
    }


    private void ReturnPlayButton()
    {
        if (playButtonAnimator.GetBool("IsBump"))
        {
            playButtonAnimator.SetBool("IsBump", false);
        }
    }

    public void ResetPause()
    {
        IsPaused=false;
    }
}