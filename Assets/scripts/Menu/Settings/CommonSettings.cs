using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonSettings : MonoBehaviour, IResetable
{
    public TMP_Dropdown StartPlaylist;
    public TMP_Dropdown StartSong;
    public TMP_InputField MusicPath;
    public TMP_InputField MusicCacheSize;
    public TMP_Text WarningMessage;
    public AudioSource MusicSource;
    void Start()
    {
        StartPlaylist.ClearOptions();
        StartSong.ClearOptions();
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        StartPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
        StartSong.SetValueWithoutNotify(Array.IndexOf(
            MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]], MusicCore.startSong));
        MusicPath.text = PathCore.MusicDirectoryPath;
        MusicCacheSize.text = MusicCore.WindowSize.ToString();
    }

    public void SetMusicDirectory()
    {
        SettingsMenu.data.MusicPath= MusicPath.text;
    }

    public void SetMusicCache()
    {
        var cache=MusicCacheSize.text;
        var cacheWindowSize = 0;
        if (int.TryParse(cache, out cacheWindowSize) && cacheWindowSize>0)
        {
            SettingsMenu.data.CacheWindowSize=cacheWindowSize;
            return;
        }
        MusicCacheSize.text = MusicCore.WindowSize.ToString();
    }

    public void SetStartPlayList()
    {
        SettingsMenu.data.StartPlayList = MusicCore.PlayListNaming[StartPlaylist.value];
        StartSong.ClearOptions();
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
    }

    public void SetStartSong()
    {
        SettingsMenu.data.StartSong =
            MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]][StartSong.value];
    }

    public void ReadMusicPath()
    {
        PathCore.MusicDirectoryPath=MusicPath.text;
        StartPlaylist.ClearOptions();
        StartSong.ClearOptions();
        MusicCore.ReadNamesOfMusic();
        var music=
        WarningMessage.text = MusicCore.MusicNameInPlaylists.Select(x => x.Value.Length).All(x => x == 0) 
            ? "Ќе найдено музыки в текущей аудитории!" 
            : "";
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
    }

    public async void UpdateMusic()
    {
        MusicCore.StopMusic(MusicSource);
        SettingsCore.SetSettings(SettingsCore.ReadSettings());
        await MusicCore.LoadStartSong();
        MusicCore.PlayMusic(MusicSource);
    }

    public void UpdateValues()
    {
        StartPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        StartSong.ClearOptions();
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
        MusicPath.text = PathCore.MusicDirectoryPath;
        MusicCacheSize.text=MusicCore.WindowSize.ToString();
    }

    
}
