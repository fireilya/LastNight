using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class MixerController : MonoBehaviour
{
    public static float MusicVolume;
    public static float SoundVolume;
    [SerializeField, FormerlySerializedAs("mixer")]
    private AudioMixerGroup mixer;

    private void Update()
    {
        mixer.audioMixer.SetFloat("musicVolume", -100 + MusicVolume);
    }
}