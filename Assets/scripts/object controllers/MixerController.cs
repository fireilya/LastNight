using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    public AudioMixerGroup Mixer;
    public static float MusicVolume;
    public static float SoundVolume;
    void Update()
    {
        Mixer.audioMixer.SetFloat("musicVolume", -100 + MusicVolume);
    }
}
