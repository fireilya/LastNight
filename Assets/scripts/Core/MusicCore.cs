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
        private static AudioClip[] AllMusic;
        private static DirectoryInfo CurrentPlayList;
        private static FileInfo[] musicFromCurrentPlaylist;
        public static bool IsStarted;

        private static AudioType[] SupportedAudioFormats =
        {
            //AudioType.OGGVORBIS,
            AudioType.MPEG
            //AudioType.WAV
        };

        private static string nakedName = "Untitled";
        public static Dictionary<string, string[]> MusicNameInPlaylists = new();
        public static List<string> PlayListNaming = new();
        public static string startPlayList;
        public static int StartSongIndex;
        public static int CurrentSongIndex;
        private static AudioClip currentAudioClip;
        public static bool IsReady;

        public static void PlayMusic(AudioSource source)
        {
            source.clip = currentAudioClip;
            source.Play();
            IsStarted = true;
        }

        public static async Task MoveMusic(bool isForward, bool playAfterMove, AudioSource source)
        {
            IsStarted = false;
            if (isForward)
            {
                CurrentSongIndex = CurrentSongIndex == musicFromCurrentPlaylist.Length - 1 ? -1 : CurrentSongIndex;
            }
            else if (CurrentSongIndex==0 && playAfterMove)
            {
                PlayMusic(source);
                return;
            }
            await DownloadNextSong(isForward);
            if (playAfterMove) PlayMusic(source);
        }

        public static async void StopMusic(AudioSource source)
        {
            source.Stop();
            IsStarted = false;
            CurrentSongIndex = -1;
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
            CurrentSongIndex = songIndex-1;
            await DownloadNextSong(true);
        }

        private static async Task DownloadNextSong(bool isRight)
        {
            var x = musicFromCurrentPlaylist;
            var y = CurrentSongIndex;
            var clip = musicFromCurrentPlaylist[isRight ? ++CurrentSongIndex : --CurrentSongIndex];
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