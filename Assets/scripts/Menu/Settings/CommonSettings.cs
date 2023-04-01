using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.scripts;
using Assets.scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CommonSettings : MonoBehaviour, IResetable
{
    [SerializeField, FormerlySerializedAs("MusicPath")]
    private TMP_InputField musicPath;

    [SerializeField, FormerlySerializedAs("MusicSource")]
    private AudioSource musicSource;

    [SerializeField, FormerlySerializedAs("StartPlaylist")]
    private TMP_Dropdown startPlaylist;

    [SerializeField, FormerlySerializedAs("StartSong")]
    private TMP_Dropdown startSong;

    [SerializeField, FormerlySerializedAs("WarningMessage")]
    private TMP_Text warningMessage;

    [SerializeField]
    private MagnitophoneController magnitophoneController;


    public void UpdateValues()
    {
        startPlaylist.ClearOptions();
        if (MusicCore.PlayListNaming.Count == 0)
        {
            startSong.ClearOptions();
            return;
        }
        startPlaylist.AddOptions(MusicCore.PlayListNaming);
        startPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        UpdateSongsDropdown();
        SettingsMenu.Data.StartSongIndex = startSong.value;
        startSong.SetValueWithoutNotify(MusicCore.StartSongIndex);
        musicPath.text = PathCore.MusicDirectoryPath;
    }

    private void Start()
    {
        startPlaylist.ClearOptions();
        startPlaylist.AddOptions(MusicCore.PlayListNaming);
        startPlaylist.SetValueWithoutNotify(Array.IndexOf(MusicCore.PlayListNaming.ToArray(), MusicCore.startPlayList));
        UpdateSongsDropdown();
        startSong.SetValueWithoutNotify(MusicCore.StartSongIndex);
        musicPath.text = PathCore.MusicDirectoryPath;
    }

    public void SetMusicDirectory()
    {
        SettingsMenu.Data.MusicPath = musicPath.text;
    }

    public void SetStartPlayList()
    {
        if (MusicCore.PlayListNaming.Count == 0) return;
        SettingsMenu.Data.StartPlayList = MusicCore.PlayListNaming[startPlaylist.value];
        UpdateSongsDropdown();
        SettingsMenu.Data.StartSongIndex = startSong.value;
    }

    public void SetStartSong()
    {
        if (MusicCore.PlayListNaming.Count!=0)
        {
            SettingsMenu.Data.StartSongIndex = startSong.value;
        }
    }

    public void ReadMusicPath()
    {
        PathCore.MusicDirectoryPath = musicPath.text;
        startPlaylist.ClearOptions();
        MusicCore.ReadNamesOfMusic();
        var music =
            warningMessage.text = MusicCore.MusicNameInPlaylists.Select(x => x.Value.Length).All(x => x == 0)
                ? "Ќе найдено музыки в текущей директории!"
                : "";
        startPlaylist.AddOptions(MusicCore.PlayListNaming);
        UpdateSongsDropdown();
        SetStartPlayList();
        SetStartSong();
    }

    public async void UpdateMusic()
    {
        var isPlaying = musicSource.isPlaying;
        MusicCore.StopMusic(musicSource);
        await MusicCore.SetPlaylist(MusicCore.PlayListNaming[startPlaylist.value], startSong.value);
        if (!isPlaying)
        {
            magnitophoneController.ResetPause();
            return;
        }

        MusicCore.PlayMusic(musicSource);
    }

    private void UpdateSongsDropdown()
    {
        startSong.ClearOptions();
        var currentPlaylist = MusicCore.PlayListNaming[startPlaylist.value];
        startSong.AddOptions(MusicCore.MusicNameInPlaylists[currentPlaylist].ToList());
        
    }
}