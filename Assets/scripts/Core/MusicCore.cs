using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.scripts
{
    public static class MusicCore
    {
        private static DirectoryInfo CurrentPlayList;
        private static FileInfo[] musicFromCurrentPlaylist;

        private static AudioType[] SupportedAudioFormats =
        {
            //AudioType.OGGVORBIS,
            AudioType.MPEG
            //AudioType.WAV
        };

        private static string nakedName = "Untitled";
        public static Dictionary<string, string[]> MusicNameInPlaylists { get; private set; } = new();
        public static List<string> PlayListNaming = new();
        public static string StartPlayList { get; set; }
        public static int StartSongIndex { get; set; }
        private static int currentSongIndex;
        private static AudioClip currentAudioClip;
        public static bool IsReady=true;

        public static void PlayMusic(AudioSourceData sourceData)
        {
            sourceData.Source.clip = currentAudioClip;
            sourceData.Source.Play();
            sourceData.IsPaused = false;
            sourceData.IsStoped=false;
            sourceData.IsStarted = true;
        }

        public static void PlayMusic(AudioSourceData sourceData, AudioClip clip)
        {
            sourceData.Source.clip = clip;
            sourceData.Source.Play();
            sourceData.IsPaused = false;
            sourceData.IsStoped = false;
        }

        public static async void MoveMusic(bool isForward, bool playAfterMove, AudioSourceData sourceData)
        {
            IsReady = false;
            if (isForward)
            {
                currentSongIndex = currentSongIndex == musicFromCurrentPlaylist.Length - 1 ? -1 : currentSongIndex;
            }
            else if (currentSongIndex==0 && playAfterMove)
            {
                PlayMusic(sourceData);
                return;
            }
            await DownloadNextSong(isForward);
            if (playAfterMove) PlayMusic(sourceData);
            IsReady=true;
        }

        public static async void StopMusic(AudioSourceData sourceData)
        {
            sourceData.Source.Stop();
            sourceData.IsStoped=true;
            sourceData.IsStarted = false;
            currentSongIndex = -1;
            await DownloadNextSong(true);
        }

        public static void ReadNamesOfMusic()
        {
            PlayListNaming = new List<string>();
            MusicNameInPlaylists = new Dictionary<string, string[]>();
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var directoryPlayLists = musicDirectory.GetDirectories();
            foreach (var playlist in directoryPlayLists)
            {
                var music = playlist.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).ToArray();
                if (music.Length != 0) PlayListNaming.Add(playlist.Name);
                var musicNames = new string[music.Length];
                for (var i = 0; i < music.Length; i++) musicNames[i] = music[i].Name;
                MusicNameInPlaylists[playlist.Name] = musicNames;
            }
            var nakedMusic=musicDirectory.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).ToArray();
            if (nakedMusic.Length == 0) return;
            MusicNameInPlaylists[nakedName] = nakedMusic.Select(x=>x.Name).ToArray();
            PlayListNaming.Add(nakedName);
        }

        public static async Task SetPlaylist(string playlistName, int songIndex=0)
        {
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var playlists = musicDirectory.GetDirectories();
            var playlistToSet = musicDirectory;
            foreach (var playlist in playlists)
                if (playlist.Name == playlistName)
                {
                    playlistToSet = playlist;
                    break;
                }
            CurrentPlayList = playlistToSet;
            musicFromCurrentPlaylist = playlistToSet.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            currentSongIndex = songIndex-1;
            await DownloadNextSong(true);

        }

        private static async Task DownloadNextSong(bool isRight)
        {
            var x = musicFromCurrentPlaylist;
            var y = currentSongIndex;
            var clip = musicFromCurrentPlaylist[isRight ? ++currentSongIndex : --currentSongIndex];
            var url = UnityWebRequestMultimedia.GetAudioClip("file:///"
                                                             + PathCore.MusicDirectoryPath
                                                             + "/"
                                                             + CurrentPlayList.Name==nakedName?"":CurrentPlayList
                                                             + "/"
                                                             + clip.Name, AudioType.MPEG);
            url.SendWebRequest();
            while (!url.isDone) await Task.Yield();
            try
            {
                var audioClip = DownloadHandlerAudioClip.GetContent(url);
                audioClip.name = clip.Name;
                currentAudioClip=audioClip;
            }
            catch (Exception)
            {
                throw new Exception($"Invalid Clip{clip.Name}");
            }
        }
    }
}