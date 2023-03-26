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
        public static string startMusic = "Angliya-Skazochniy Mir.mp3";
        public static string startPlayList = "menu";
        private static MusicWindow musicWindow;
        public static int windowSize = 9;
        private static int rightEdgeSong;
        private static int leftEdgeSong;

        public static void PlayMusic(AudioSource source)
        {
            source.clip = musicWindow.CurrentNode.Value;
            source.Play();
            IsStarted = true;
        }

        public static void MoveMusic(bool isForward, bool playAfterMove, AudioSource source)
        {
            if (isForward)
                musicWindow.ShiftRight();
            else
                musicWindow.ShiftLeft();
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
                if (clip.name == startMusic)
                {
                    musicWindow.SetOutToIndex(index);
                    return;
                }

                index++;
            }

            index--;
            musicWindow.SetOutToIndex(index);
            while (musicWindow.CurrentNode.Value.name != startMusic && index < musicFromCurrentPlaylist.Length)
            {
                musicWindow.ShiftRight();
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
                    playlistToSet = playlist;
            CurrentPlayList = playlistToSet;
            musicFromCurrentPlaylist = playlistToSet.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
            musicWindow = new MusicWindow(windowSize, musicFromCurrentPlaylist.Length);
            rightEdgeSong = 0;
            leftEdgeSong = 0;
            await FillWindow();
        }

        public static async Task<AudioClip> DownloadNextSong(bool isRight)
        {
            var clip = musicFromCurrentPlaylist[isRight ? rightEdgeSong++ : leftEdgeSong++];
            var url = UnityWebRequestMultimedia.GetAudioClip("file:///" 
                                                             + PathCore.MusicDirectoryPath 
                                                             + "/" 
                                                             + CurrentPlayList.Name 
                                                             + "/" 
                                                             + clip.Name, AudioType.MPEG);
            url.SendWebRequest();
            while (!url.isDone) await Task.Yield();
            var audioClip = DownloadHandlerAudioClip.GetContent(url);
            audioClip.name = clip.Name;
            return audioClip;
        }

        public static async Task FillWindow()
        {
            musicWindow.Clear();
            for (var i = 0; i <= musicWindow.Size; i++)
            {
                if (rightEdgeSong == musicFromCurrentPlaylist.Length) break;
                musicWindow.AddLast(await DownloadNextSong(true));
            }
        }
    }
}