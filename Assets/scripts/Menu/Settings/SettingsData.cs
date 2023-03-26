using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public string StartPlayList;
    public string StartSong;
    public string MusicPath;
    public int CacheWindowSize;
    public float GlobalMusicVolume;
    public float GlobalSoundVolume;

    public SettingsData()
    {
        
    }
}
