using System;

[Serializable]
public class SettingsData
{
    public float GlobalMusicVolume;
    public float GlobalSoundVolume;
    public bool IsFullScreen;
    public string MusicPath;
    public int ResolutionHeight;
    public int ResolutionWidth;
    public string StartPlayList;
    public int StartSongIndex;

    public static bool operator ==(SettingsData x, SettingsData y)
    {
        return x.StartSongIndex == y.StartSongIndex
               && x.StartPlayList == y.StartPlayList
               && x.MusicPath == y.MusicPath
               && Math.Abs(x.GlobalMusicVolume - y.GlobalMusicVolume) < 1e-3
               && Math.Abs(x.GlobalSoundVolume - y.GlobalSoundVolume) < 1e-3
               && x.IsFullScreen == y.IsFullScreen
               && x.ResolutionHeight == y.ResolutionHeight
               && x.ResolutionWidth == y.ResolutionWidth;
    }

    public static bool operator !=(SettingsData x, SettingsData y)
    {
        return !(x.StartSongIndex == y.StartSongIndex
                 && x.StartPlayList == y.StartPlayList
                 && x.MusicPath == y.MusicPath
                 && Math.Abs(x.GlobalMusicVolume - y.GlobalMusicVolume) < 1e-3
                 && Math.Abs(x.GlobalSoundVolume - y.GlobalSoundVolume) < 1e-3
                 && x.IsFullScreen == y.IsFullScreen
                 && x.ResolutionHeight == y.ResolutionHeight
                 && x.ResolutionWidth == y.ResolutionWidth);
    }
}