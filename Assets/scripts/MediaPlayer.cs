using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject Fill;
    public static bool isStarted;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VideoPlayer.isPlaying)
        {
            isStarted = true;
        }
        if (!VideoPlayer.isPlaying && isStarted || Input.anyKeyDown)
        {
            Fill.SetActive(false);
            VideoPlayer.gameObject.SetActive(false);
        }
    }
}
