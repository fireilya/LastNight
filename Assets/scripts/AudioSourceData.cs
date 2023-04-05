using System.Collections;
using UnityEngine;

namespace Assets.scripts
{
    public class AudioSourceData
    {
        public AudioSource Source { get; set; }
        public bool IsPaused;
        public bool IsStoped;
        public bool IsStarted;

        public AudioSourceData(AudioSource source)
        {
            Source=source;
            IsPaused=false;
            IsStoped=false;
            IsStarted=false;
        }

    }
}