using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    public static class SettingsCore
    {

        public static void WriteSettings(SettingsData dataToSave)
        {
            var formatter=new BinaryFormatter();
            using var stream=new FileStream(PathCore.SettingsFilePath, FileMode.Create);
            formatter.Serialize(stream, dataToSave);
        }

        public static SettingsData ReadSettings()
        {
            var formatter = new BinaryFormatter();
            if (!File.Exists(PathCore.SettingsFilePath)) return null;
            using var stream = new FileStream(PathCore.SettingsFilePath, FileMode.Open);
            var settings = formatter.Deserialize(stream) as SettingsData;
            return settings;
        }

        public static SettingsData ReadDefaultSettings()
        {
            return null;
        }
    }
}
