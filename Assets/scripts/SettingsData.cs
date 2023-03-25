using System;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public static string StartPlayList;
    public static string StartSong;

    public SettingsData(string a, string b)
    {
        StartPlayList=a; StartSong=b;
    }
}
