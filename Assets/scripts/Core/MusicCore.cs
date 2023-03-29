using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private static MusicWindow musicWindow;
        public static int WindowSize;
        private static int rightEdgeSong;
        private static int leftEdgeSong;
        public static bool IsReady;

        public static void PlayMusic(AudioSource source)
        {
            source.clip = musicWindow.CurrentNode.Value;
            source.Play();
            IsStarted = true;
        }

        public static async Task MoveMusic(bool isForward, bool playAfterMove, AudioSource source)
        {
            IsReady = false;
            IsStarted = false;
            if (isForward)
                await musicWindow.Next();
            else
                await musicWindow.Previous();
            if (playAfterMove) PlayMusic(source);
        }

        public static async void StopMusic(AudioSource source)
        {
            source.Stop();
            IsStarted = false;
            await Task.Run(FillWindow);
            //FillWindowBackground();
        }

        public static void ReadNamesOfMusic()
        {
            PlayListNaming = new List<string>();
            MusicNameInPlaylists = new Dictionary<string, string[]>();
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var directoryPlayLists = musicDirectory.GetDirectories();
            foreach (var playlist in directoryPlayLists)
            {
                var music = playlist.GetFiles("*.ogg", SearchOption.TopDirectoryOnly)
                    .Union(playlist.GetFiles("*.mp3", SearchOption.TopDirectoryOnly)
                        .Union(playlist.GetFiles("*.wav", SearchOption.TopDirectoryOnly)))
                    .ToArray();
                if (music.Length != 0) PlayListNaming.Add(playlist.Name);
                var musicNames = new string[music.Length];
                for (var i = 0; i < music.Length; i++) musicNames[i] = music[i].Name;
                MusicNameInPlaylists[playlist.Name] = musicNames;
            }
        }

        public static async Task LoadStartSong()
        {
            await Task.Factory.StartNew(() => SetPlaylist(startPlayList));
            Debug.Log("StartLoad");
            var index = 0;
            foreach (var clip in musicWindow)
            {
                if (clip.name == startSong)
                {
                    musicWindow.SetOutToIndex(index);
                    return;
                }

                index++;
            }

            index--;
            musicWindow.SetOutToIndex(index);
            while (musicWindow.CurrentNode.Value.name != startSong && index < musicFromCurrentPlaylist.Length)
            {
                await musicWindow.Next();
                index++;
            }
        }

        public static async void SetPlaylist(string playlistName)  
        {
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var playlists = musicDirectory.GetDirectories();
            var playlistToSet = playlists[0];
            foreach (var playlist in playlists)
            {
                Debug.Log(playlist.Name);
                if (playlist.Name == playlistName)
                {
                    playlistToSet = playlist;
                    break;
                }
            }

            CurrentPlayList = playlistToSet;
            musicFromCurrentPlaylist = playlistToSet.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            Debug.Log("New");
            var task = Task.Factory.StartNew(FillWindow, TaskCreationOptions.AttachedToParent);
            task.
        }

        public static async Task<AudioClip> DownloadNextSong(bool isRight)
        {
            var index = isRight ? rightEdgeSong++ : leftEdgeSong++;
            var l = musicFromCurrentPlaylist.Length;
            var x = musicFromCurrentPlaylist; 
            var clip = musicFromCurrentPlaylist[index];
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
                return audioClip;
            }
            catch (Exception)
            {
                throw new Exception($"InvalidClip{clip.Name}");
            }
        }

        public static async void FillWindow()
        {
            musicWindow?.Dispose();
            musicWindow = new MusicWindow(WindowSize * 2 + 1, musicFromCurrentPlaylist.Length);
            rightEdgeSong = 0;
            leftEdgeSong = 0;
            for (var i = 0; i <= musicWindow.Size; i++)
            {
                Debug.Log(i.ToString());
                if (rightEdgeSong == musicFromCurrentPlaylist.Length) break;
                await DownloadNextSong(true);
                //musicWindow.AddLast(audioclipTask.Result);
            }
        }
    }
}