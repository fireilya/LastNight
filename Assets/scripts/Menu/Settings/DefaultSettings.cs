using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    public class DefaultSettings
    {
        private static SettingsData data=new();
        public static void CreateDefaultSettings()
        {
            data.StartPlayList = "menu";
            data.StartSong = "Angliya-Skazochniy Mir.mp3";
            data.MusicPath= Application.dataPath + @"/Music";
            data.CacheWindowSize = 9;
            data.GlobalMusicVolume = 100f;
            data.GlobalSoundVolume = 1f;
            SettingsCore.WriteSettingsTo(data, PathCore.DefaultSettingsFilePath);
        }
    }
}
