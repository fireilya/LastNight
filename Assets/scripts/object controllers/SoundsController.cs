using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public enum Sounds
{
    PressButtonLatch = 0,
    MoveMusic = 1,
    Transition = 2,
    EnterTik=3,
    OutTik=4
}

namespace Assets.scripts
{
    public class SoundsController:MonoBehaviour
    {
        public AudioClip[] FX;
        public AudioClip[] UI;
        public AudioMixerGroup Mixer;
        public void PlaySound(AudioSource source, AudioClip[] type, Sounds sound, float percentVolume=100)
        {
            source.clip = type[(int)sound];
            Mixer.audioMixer.SetFloat("soundVolume", -100 + percentVolume);
            source.Play();
        }
    }
}
