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
        private static MusicHistory History=new();
        private static int songIndex;

        public static void PlayMusic(AudioSource source)
        {
            source.clip = History.CurrentNode.Value;
            source.Play();
            IsStarted=true;
        }

        public static void MoveMusic(int direction, bool playAfterMove, AudioSource source)
        {
            songIndex += direction;
            songIndex = songIndex < 0 ? 0 : songIndex;
            if (playAfterMove) PlayMusic(source);
        }

        public static void StopMusic(AudioSource source)
        {
            History.CurrentNode = History.FirstNode;
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

        public static async void LoadStartSong()
        {
            SetPlaylist(startPlayList);
            while (MusicFromCurrentPlaylist[songIndex].Name!=startMusic && songIndex<MusicFromCurrentPlaylist.Length)
            {
                History.Add(await DownloadNextSong());
                songIndex++;
            }
            History.Add(await DownloadNextSong());
        }

        public static void SetPlaylist(string playlistName)
        {
            History.Clear();
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
        }

        private static async Task<AudioClip> DownloadNextSong()
        {
            var clip = MusicFromCurrentPlaylist[songIndex];
            var url = UnityWebRequestMultimedia.GetAudioClip("file:///" + PathCore.MusicDirectoryPath + "/" + CurrentPlayList.Name + "/" +
                                                             clip.Name, AudioType.MPEG);
            url.SendWebRequest();
            while (!url.isDone)
            {
                await Task.Yield();
            }
            return DownloadHandlerAudioClip.GetContent(url);
        }
    }
}

