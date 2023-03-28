using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    public static float MusicVolume;
    public static float SoundVolume;
    public AudioMixerGroup Mixer;

    private void Update()
    {
        Mixer.audioMixer.SetFloat("musicVolume", -100 + MusicVolume);
    }
}