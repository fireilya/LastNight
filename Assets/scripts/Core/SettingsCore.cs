using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.scripts
{
    public static class SettingsCore
    {
        private static readonly BinaryFormatter formatter = new();

        public static void WriteSettingsTo(SettingsData dataToSave, string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, dataToSave);
            stream.Close();
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
            SettingsMenu.Data=data;
            MusicCore.StartPlayList = data.StartPlayList;
            MusicCore.StartSongIndex = data.StartSongIndex;
            PathCore.MusicDirectoryPath = data.MusicPath;
            MixerController.SoundVolume = data.GlobalSoundVolume;
            MixerController.MusicVolume = data.GlobalMusicVolume;
        }
    }
}