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

    public static bool operator == (SettingsData x, SettingsData y)
    {
        return x.StartSong == y.StartSong
               && x.StartPlayList == y.StartPlayList
               && x.MusicPath == y.MusicPath
               && x.CacheWindowSize == y.CacheWindowSize
               && x.GlobalMusicVolume == y.GlobalMusicVolume
               && x.GlobalSoundVolume == y.GlobalSoundVolume;
    }

    public static bool operator !=(SettingsData x, SettingsData y)
    {
        return !(x.StartSong == y.StartSong
                 && x.StartPlayList == y.StartPlayList
                 && x.MusicPath == y.MusicPath
                 && x.CacheWindowSize == y.CacheWindowSize
                 && x.GlobalMusicVolume == y.GlobalMusicVolume
                 && x.GlobalSoundVolume == y.GlobalSoundVolume);
    }
}
