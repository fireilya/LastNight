using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundFXLibrary", menuName = "ScriptableObjects/SoundFXLibrary")]
public class SoundFXLibrary : ScriptableObject
{
    public AudioClip[] UI;
    public AudioClip[] FX;

}
