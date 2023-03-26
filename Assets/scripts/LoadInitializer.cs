using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class LoadInitializer : MonoBehaviour
    {
        private BackgroundWorker backgroundInitializator=new();
        public TMP_Text message;

        async void Awake()
        {
            message.text = "Создаю резервную копию";
            DefaultSettings.CreateDefaultSettings();
            message.text = "Просматриваю музыку";
            MusicCore.ReadNamesOfMusic();
            message.text = "Читаю настройки";
            SettingsCore.SetSettings(SettingsCore.ReadSettings());
            message.text = "Инициализирую музыку";
            await MusicCore.LoadStartSong();
            message.text = "Загружаю меню";
            SceneManager.LoadScene(1);
        }
    }
}
