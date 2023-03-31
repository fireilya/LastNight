using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.scripts;
using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;

public class CommonSettings : MonoBehaviour, IResetable
{
    public TMP_InputField MusicPath;
    public AudioSource MusicSource;
    public TMP_Dropdown StartPlaylist;
    public TMP_Dropdown StartSong;
    public TMP_Text WarningMessage;

    public void UpdateValues()
    {
        StartPlaylist.ClearOptions();
        if (MusicCore.PlayListNaming.Count == 0)
        {
            StartSong.ClearOptions();
            return;
        }
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        StartPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        UpdateSongsDropdown();
        StartSong.SetValueWithoutNotify(MusicCore.StartSongIndex);
        MusicPath.text = PathCore.MusicDirectoryPath;
    }

    private void Start()
    {
        StartPlaylist.ClearOptions();
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        StartPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        UpdateSongsDropdown();
        StartSong.SetValueWithoutNotify(MusicCore.StartSongIndex);
        MusicPath.text = PathCore.MusicDirectoryPath;
    }

    public void SetMusicDirectory()
    {
        SettingsMenu.data.MusicPath = MusicPath.text;
    }

    public void SetStartPlayList()
    {
        if (MusicCore.PlayListNaming.Count == 0) return;
        SettingsMenu.data.StartPlayList = MusicCore.PlayListNaming[StartPlaylist.value];
        UpdateSongsDropdown();
    }

    public void SetStartSong()
    {
        if (MusicCore.PlayListNaming.Count!=0)
        {
            SettingsMenu.data.StartSongIndex = StartSong.value;
        }
    }

    public void ReadMusicPath()
    {
        PathCore.MusicDirectoryPath = MusicPath.text;
        StartPlaylist.ClearOptions();
        MusicCore.ReadNamesOfMusic();
        var music =
            WarningMessage.text = MusicCore.MusicNameInPlaylists.Select(x => x.Value.Length).All(x => x == 0)
                ? "Ќе найдено музыки в текущей директории!"
                : "";
        StartPlaylist.AddOptions(MusicCore.PlayListNaming);
        UpdateSongsDropdown();
        SetStartPlayList();
        SetStartSong();
    }

    public async void UpdateMusic()
    {
        MusicCore.StopMusic(MusicSource);
        await MusicCore.SetPlaylist(MusicCore.PlayListNaming[StartPlaylist.value], StartSong.value);
        MusicCore.PlayMusic(MusicSource);
    }

    private void UpdateSongsDropdown()
    {
        StartSong.ClearOptions();
        var currentPlaylist = MusicCore.PlayListNaming[StartPlaylist.value];
        StartSong.AddOptions(MusicCore.MusicNameInPlaylists[currentPlaylist].ToList());
        SettingsMenu.data.StartSongIndex = StartSong.value;
    }
}