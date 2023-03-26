using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.scripts
{
    public static class SettingsCore
    {
        private static BinaryFormatter formatter = new();
        //public CommonSettings CommonSettings;
        //public GameObject CommonSettings;
        //public GameObject CommonSettings;
        //public GameObject CommonSettings;

        public static void WriteSettingsTo(SettingsData dataToSave, string path)
        {
            
            using var stream=new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, dataToSave);
        }

        public static SettingsData ReadSettings()
        {
            var stream = !File.Exists(PathCore.SettingsFilePath)
                ? new FileStream(PathCore.DefaultSettingsFilePath, FileMode.Open)
                : new FileStream(PathCore.SettingsFilePath, FileMode.Open);
            var settings = formatter.Deserialize(stream) as SettingsData;

            stream.Close();
            return settings;

        }

        public static SettingsData ReadDefaultSettings()
        {
            var stream = new FileStream(PathCore.DefaultSettingsFilePath, FileMode.Open);
            var settings = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
            return settings;
        }

        public static void SetSettings(SettingsData data)
        {
            SettingsMenu.data=data;
            MusicCore.startPlayList = data.StartPlayList;
            MusicCore.startSong = data.StartSong;
            PathCore.MusicDirectoryPath = data.MusicPath;
            MusicCore.windowSize = data.CacheWindowSize;
            MixerController.SoundVolume = data.GlobalSoundVolume;
            MixerController.MusicVolume = data.GlobalMusicVolume;
        }
    }
}
