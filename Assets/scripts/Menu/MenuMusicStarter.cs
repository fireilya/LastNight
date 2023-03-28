using System.Collections;
using UnityEngine;

namespace Assets.scripts
{
    internal class MenuMusicStarter : MonoBehaviour
    {
        public MagnitophoneController Magnitophone;

        private void Awake()
        {
            StartCoroutine(Starter());
        }

        private IEnumerator Starter()
        {
            while (!MediaPlayer.IsStarted) yield return null;
            yield return StartCoroutine(Magnitophone.PressPlayButton());
        }
    }
}