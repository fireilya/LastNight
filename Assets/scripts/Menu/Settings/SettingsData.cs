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
    public string StartSong;

    public static bool operator ==(SettingsData x, SettingsData y)
    {
        return x.StartSong == y.StartSong
               && x.StartPlayList == y.StartPlayList
               && x.MusicPath == y.MusicPath
               && x.GlobalMusicVolume == y.GlobalMusicVolume
               && x.GlobalSoundVolume == y.GlobalSoundVolume
               && x.IsFullScreen == y.IsFullScreen
               && x.ResolutionHeight == y.ResolutionHeight
               && x.ResolutionWidth == y.ResolutionWidth;
    }

    public static bool operator !=(SettingsData x, SettingsData y)
    {
        return !(x.StartSong == y.StartSong
                 && x.StartPlayList == y.StartPlayList
                 && x.MusicPath == y.MusicPath
                 && x.GlobalMusicVolume == y.GlobalMusicVolume
                 && x.GlobalSoundVolume == y.GlobalSoundVolume
                 && x.IsFullScreen == y.IsFullScreen
                 && x.ResolutionHeight == y.ResolutionHeight
                 && x.ResolutionWidth == y.ResolutionWidth);
    }
}