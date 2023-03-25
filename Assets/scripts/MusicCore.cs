using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.scripts
{
    public class MusicCore : MonoBehaviour
    {
        private static AudioClip[] AllMusic;
        private static DirectoryInfo CurrentPlayList;
        private static FileInfo[] MusicFromCurrentPlaylist;
        public static bool IsStarted=false;
        private static AudioType[] SupportedAudioFormats = new AudioType[]
        {
            //AudioType.OGGVORBIS,
            AudioType.MPEG,
            //AudioType.WAV
        };
        public static Dictionary<string, string[]> MusicNameInPlaylists=new();
        public static List<string> PlayListNaming = new();
        public static string startMusic = "Angliya-Skazochniy Mir.mp3";
        public static string startPlayList = "menu";
        private static Window<AudioClip> musicWindow;
        private static int windowSize = 9;
        private static int rightEdgeSong;
        private static int leftEdgeSong;

        public static void PlayMusic(AudioSource source)
        {
            source.clip = musicWindow.CurrentNode.Value;
            source.Play();
            IsStarted=true;
        }

        public static void MoveMusic(int direction, bool playAfterMove, AudioSource source)
        {
            rightEdgeSong += direction;
            rightEdgeSong = rightEdgeSong < 0 ? 0 : rightEdgeSong;
            if (playAfterMove) PlayMusic(source);
        }

        public static void StopMusic(AudioSource source)
        {
            musicWindow.CurrentNode = musicWindow.FirstNode;
            source.Stop();
        }

        public static void ReadNamesOfMusic()
        {
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var directoryPlayLists = musicDirectory.GetDirectories();
            foreach (var playlist in directoryPlayLists)
            {
                PlayListNaming.Add(playlist.Name);
                var music = playlist.GetFiles("*.ogg", SearchOption.TopDirectoryOnly)
                    .Union(playlist.GetFiles("*.mp3", SearchOption.TopDirectoryOnly)
                        .Union(playlist.GetFiles("*.wav", SearchOption.TopDirectoryOnly)))
                    .ToArray();
                var musicNames = new string[music.Length];
                for (var i = 0; i < music.Length; i++)
                {
                    musicNames[i] = music[i].Name;
                }
                MusicNameInPlaylists[playlist.Name]=musicNames;
            }
        }

        public static void LoadStartSong()
        {
            SetPlaylist(startPlayList);
            var index = 0;
            if (musicWindow.Any(music => music.name==startMusic))
            {
                musicWindow.SetOutToIndex(index);
            }
        }

        public static void SetPlaylist(string playlistName)
        {
            var musicDirectory = new DirectoryInfo(PathCore.MusicDirectoryPath);
            var playlists = musicDirectory.GetDirectories();
            var playlistToSet = playlists[0];
            foreach (var playlist in playlists)
            {
                if (playlist.Name == playlistName)
                {
                    playlistToSet = playlist;
                }
            }
            CurrentPlayList=playlistToSet;
            MusicFromCurrentPlaylist = playlistToSet.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            musicWindow = new Window<AudioClip>(windowSize, MusicFromCurrentPlaylist.Length);
            FillWindow();
        }

        private static async Task<AudioClip> DownloadNextSong(bool isRight)
        {
            var nextSongIndex=isRight ? rightEdgeSong : leftEdgeSong;
            var clip = MusicFromCurrentPlaylist[nextSongIndex];
            var url = UnityWebRequestMultimedia.GetAudioClip("file:///" + PathCore.MusicDirectoryPath + "/" +
                                                             CurrentPlayList.Name + "/" +
                                                             clip.Name, AudioType.MPEG);
            url.SendWebRequest();
            while (!url.isDone) await Task.Yield();
            var audioClip = DownloadHandlerAudioClip.GetContent(url);
            audioClip.name = clip.Name;
            nextSongIndex++;
            if (isRight)
            {
                rightEdgeSong=nextSongIndex;
            }
            else
            {
                leftEdgeSong=nextSongIndex;
            }
            return audioClip;
        }

        public static async void FillWindow()
        {
            musicWindow.Clear();
            for (var i = 0; i <= musicWindow.Size; i++)
            {
                musicWindow.AddLast(await DownloadNextSong(true));
            }
        }
    }
}

