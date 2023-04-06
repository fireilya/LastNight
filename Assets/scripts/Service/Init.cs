using UnityEngine;

namespace Assets.scripts.Service
{
    public class Init : MonoBehaviour
    {
        private async void Awake()
        {
            DefaultSettings.CreateDefaultSettings();
            SettingsCore.SetSettings(SettingsCore.ReadSettings());
            MusicCore.ReadNamesOfMusic();
            await MusicCore.SetPlaylist(MusicCore.StartPlayList, MusicCore.StartSongIndex);
        }
    }
}
