using System;
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
        public static int WindowSize = 9;
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

        public static void StopMusic(AudioSource source)
        {
            IsStarted = false;
            musicWindow.CurrentNode = musicWindow.FirstNode;
            source.Stop();
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
            await SetPlaylist(startPlayList);
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
            await FillWindow();
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

        public static async Task FillWindow()
        {
            musicWindow = new MusicWindow(WindowSize * 2 + 1, musicFromCurrentPlaylist.Length);
            rightEdgeSong = 0;
            leftEdgeSong = 0;
            for (var i = 0; i <= musicWindow.Size; i++)
            {
                if (rightEdgeSong == musicFromCurrentPlaylist.Length) break;
                musicWindow.AddLast(await DownloadNextSong(true));
            }
        }
    }
}