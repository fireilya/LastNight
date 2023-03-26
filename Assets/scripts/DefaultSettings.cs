using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    public class DefaultSettings
    {
        private static SettingsData _data=new();
        public static void CreateDefaultSettings()
        {
            _data.StartPlayList = "menu";
            _data.StartSong = "Angliya-Skazochniy Mir.mp3";
            SettingsCore.WriteSettingsTo(_data, PathCore.DefaultSettingsFilePath);
        }
    }
}
