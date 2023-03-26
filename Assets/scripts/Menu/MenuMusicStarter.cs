using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    class MenuMusicStarter:MonoBehaviour
    {
        public MagnitophoneController Magnitophone;
        void Awake()
        {
            StartCoroutine(Starter());
        }

        IEnumerator Starter()
        {
            while (!MediaPlayer.IsStarted)
            {
                yield return null;
            }
            yield return StartCoroutine(Magnitophone.PressPlayButton());
        }
    }
}
