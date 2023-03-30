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
    public class MusicCore : MonoBehaviour
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

        public static Dictionary<string, string[]> MusicNameInPlaylists = new();
        public static List<string> PlayListNaming = new();
        public static string startSong = "Angliya-Skazochniy Mir.mp3";
        public static string startPlayList = "menu";
        private static int currentSongIndex;
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
                currentSongIndex %= musicFromCurrentPlaylist.Length;
            }
            else
                currentSongIndex=currentSongIndex == 0 ? currentSongIndex : --currentSongIndex;
            await DownloadNextSong(isForward);
            if (playAfterMove) PlayMusic(source);
        }

        public static async void StopMusic(AudioSource source)
        {
            source.Stop();
            IsStarted = false;
            currentSongIndex = 0;
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
        }

        public static async Task LoadStartSong(TMP_Text progressMessage)
        {  
            await SetPlaylist(startPlayList);
            while (currentAudioClip.name!=startSong && currentSongIndex<musicFromCurrentPlaylist.Length)
            {
                await DownloadNextSong(true);
                progressMessage.text = currentAudioClip.name;
            }

            progressMessage.text = "";
        }

        public static async Task SetPlaylist(string playlistName)  
        {
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var playlists = musicDirectory.GetDirectories();
            var playlistToSet = playlists[0];
            foreach (var playlist in playlists)
                if (playlist.Name == playlistName)
                {
                    playlistToSet = playlist;
                    break;
                }
            CurrentPlayList = playlistToSet;
            musicFromCurrentPlaylist = playlistToSet.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            currentSongIndex = 0;
            await DownloadNextSong(true);
        }

        private static async Task DownloadNextSong(bool isRight)
        {
            var clip = musicFromCurrentPlaylist[isRight ? currentSongIndex++ : currentSongIndex--];
            var url = UnityWebRequestMultimedia.GetAudioClip("file:///"
                                                             + PathCore.MusicDirectoryPath
                                                             + "/"
                                                             + CurrentPlayList.Name
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
                throw new Exception($"InvalidClip{clip.Name}");
            }
        }
    }
}