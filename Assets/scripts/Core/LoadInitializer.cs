using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.scripts
{
    public class LoadInitializer : MonoBehaviour
    {
        private BackgroundWorker backgroundInitializator = new();
        public TMP_Text message;

        private async void Awake()
        {
            message.text = "Создаю резервную копию...";
            DefaultSettings.CreateDefaultSettings();
            message.text = "Читаю настройки...";
            SettingsCore.SetSettings(SettingsCore.ReadSettings());
            message.text = "Читаю музыку...";
            MusicCore.ReadNamesOfMusic();
            message.text = "Инициализирую музыку...";
            await MusicCore.LoadStartSong();
            message.text = "Загружаю меню...";
            SceneManager.LoadScene(1);
        }
    }
}