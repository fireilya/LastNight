using UnityEngine;
using UnityEngine.Video;

public class MediaPlayer : MonoBehaviour
{
    public static bool IsStarted;
    public GameObject Fill;
    public VideoPlayer VideoPlayer;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (VideoPlayer.isPlaying) IsStarted = true;
        if ((!VideoPlayer.isPlaying && IsStarted) || Input.anyKeyDown)
        {
            Fill.SetActive(false);
            VideoPlayer.gameObject.SetActive(false);
        }
    }
}