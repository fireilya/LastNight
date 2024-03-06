using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class MediaPlayer : MonoBehaviour
{
    public static bool IsStarted;

    [SerializeField, FormerlySerializedAs("Fill")]
    private GameObject fill;

    [SerializeField, FormerlySerializedAs("VideoPlayer")]
    private VideoPlayer videoPlayer;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (videoPlayer.isPlaying) IsStarted = true;
        if ((!videoPlayer.isPlaying && IsStarted) || Input.anyKeyDown)
        {
            fill.SetActive(false);
            videoPlayer.gameObject.SetActive(false);
        }
    }
}