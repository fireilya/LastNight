using Assets.scripts.Enum;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Assets.scripts
{
    public class SoundsController : MonoBehaviour
    {
        public SoundFXLibrary Library;

        [SerializeField, FormerlySerializedAs("Mixer")]
        private AudioMixerGroup mixer;

        public void PlaySound(AudioSource source, AudioClip[] type, Sounds sound, float percentVolume = 100)
        {
            source.clip = type[(int)sound];
            var Volume = -100 + percentVolume * MixerController.SoundVolume;
            mixer.audioMixer.SetFloat("soundVolume", -100 + percentVolume * MixerController.SoundVolume);
            source.Play();
        }
    }
}