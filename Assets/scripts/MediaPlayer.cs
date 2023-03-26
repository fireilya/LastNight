using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject Fill;
    public static bool IsStarted;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VideoPlayer.isPlaying)
        {
            IsStarted = true;
        }
        if (!VideoPlayer.isPlaying && IsStarted || Input.anyKeyDown)
        {
            Fill.SetActive(false);
            VideoPlayer.gameObject.SetActive(false);
        }
    }
}
