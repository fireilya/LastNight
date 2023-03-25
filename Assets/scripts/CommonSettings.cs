using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonSettings : MonoBehaviour
{
    public TMP_Dropdown StartPlaylist;
    public TMP_Dropdown StartSong;

    void Start()
    {
        StartPlaylist.ClearOptions();
        StartSong.ClearOptions();
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
        SettingsData.StartPlayList = MusicCore.PlayListNaming[StartPlaylist.value];
        SettingsData.StartSong =
            MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]][StartSong.value];
    }

    public void SetStartPlayList()
    {
        SettingsData.StartPlayList = MusicCore.PlayListNaming[StartPlaylist.value];
        StartSong.ClearOptions();
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]].ToList());
    }

    public void SetStartSong()
    {
        SettingsData.StartSong =
            MusicCore.MusicNameInPlaylists[MusicCore.PlayListNaming[StartPlaylist.value]][StartSong.value];
    }
}
