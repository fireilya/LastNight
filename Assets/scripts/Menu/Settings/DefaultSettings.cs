using UnityEngine;

namespace Assets.scripts
{
    public class DefaultSettings
    {
        private static readonly SettingsData data = new();

        public static void CreateDefaultSettings()
        {
            data.StartPlayList = "menu";
            data.StartSong = "Angliya-Skazochniy Mir.mp3";
            data.MusicPath = Application.dataPath + @"/Music";
            data.CacheWindowSize = 9;
            data.GlobalMusicVolume = 100f;
            data.GlobalSoundVolume = 1f;
            data.ResolutionWidth = Screen.currentResolution.width;
            data.ResolutionHeight = Screen.currentResolution.height;
            data.IsFullScreen = true;
            SettingsCore.WriteSettingsTo(data, PathCore.DefaultSettingsFilePath);
        }
    }
}