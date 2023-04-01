using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public enum Sounds
{
    PressButtonLatch = 0,
    MoveMusic = 1,
    Transition = 2,
    EnterTik = 3,
    OutTik = 4
}

namespace Assets.scripts
{
    public class SoundsController : MonoBehaviour
    {

        public AudioClip[] FX;

        [SerializeField, FormerlySerializedAs("Mixer")]
        private AudioMixerGroup mixer;

        public AudioClip[] UI;

        public void PlaySound(AudioSource source, AudioClip[] type, Sounds sound, float percentVolume = 100)
        {
            source.clip = type[(int)sound];
            var Volume = -100 + percentVolume * MixerController.SoundVolume;
            mixer.audioMixer.SetFloat("soundVolume", -100 + percentVolume * MixerController.SoundVolume);
            source.Play();
        }
    }
}