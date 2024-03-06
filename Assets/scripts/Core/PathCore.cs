using UnityEngine;

namespace Assets.scripts
{
    public static class PathCore
    {
        public static string MusicDirectoryPath = Application.dataPath + @"/Music";
        public static string SettingsFilePath = Application.dataPath + @"/conf/CFG.fi";
        public static string DefaultSettingsFilePath = Application.dataPath + @"/conf/defCFG.fi";
        public static string PlayerSavePath = Application.dataPath + @"/Saves";
    }
}