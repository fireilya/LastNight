using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Searcher : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField input;

    [SerializeField]
    private TMP_Dropdown songMenu;

    [SerializeField]
    private CommonSettings commonSettings;

    [SerializeField]
    private GameObject searcher;

    private List<string> dropdownOptions = new();

    private List<(string playlist, string song)> chooseData=new();

    void Start()
    {
        songMenu.ClearOptions();
    }

    public void Apply()
    {
        commonSettings.SetStartPlayList(MusicCore.PlayListNaming.IndexOf(chooseData[songMenu.value].playlist));
        commonSettings.SetStartSong(
            Array.IndexOf(MusicCore.MusicNameInPlaylists[chooseData[songMenu.value].playlist],
                chooseData[songMenu.value].song)
            );
        searcher.SetActive(false);
    }

    public void StartSearch()
    {
        songMenu.ClearOptions();
        dropdownOptions.Clear();
        chooseData.Clear();
        var playlists = MusicCore.MusicNameInPlaylists.Keys.ToArray();
        var parallelWorkers = new Task[playlists.Length];
        for (var i = 0; i < playlists.Length; i++)
        {
            var temp = i;
            parallelWorkers[i] = new Task(()=>ProcessPlayList(playlists[temp]));
            parallelWorkers[i].Start();
        }

        Task.WaitAll(parallelWorkers);
        dropdownOptions.Sort();
        songMenu.AddOptions(dropdownOptions);
    }

    private void ProcessPlayList(string playlistName)
    {
        var substring = input.text.ToLower();
        foreach (var song in MusicCore.MusicNameInPlaylists[playlistName])
        {
            if (!song.ToLower().Contains(substring)) continue;
            dropdownOptions.Add($"({playlistName}) {song}");
            chooseData.Add((playlistName, song));
        }
    }
}
