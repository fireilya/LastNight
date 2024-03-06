using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.scripts
{
    internal class MenuMusicStarter : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("Magnitophone")]
        private MagnitophoneController magnitophone;

        private void Awake()
        {
            StartCoroutine(Starter());
        }

        private IEnumerator Starter()
        {
            while (!MediaPlayer.IsStarted) yield return null;
            yield return StartCoroutine(magnitophone.PressPlayButton());
        }
    }
}