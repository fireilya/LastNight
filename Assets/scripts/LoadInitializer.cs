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
        public TextMeshProUGUI message;

        async void Awake()
        {

            //backgroundInitializator.RunWorkerAsync(StartCoroutine(Initialize()));
            MusicCore.ReadNamesOfMusic();
            await MusicCore.LoadStartSong();
            SceneManager.LoadScene(1);
        }

        //private IEnumerator Initialize()
        //{
        //    message.text = "Инициализирую музыку";
        //    yield return StartCoroutine(MusicCore.InitializeMusic());
        //    message.text = "Загружаю сцену";
        //    SceneManager.LoadScene(1);
        //}
    }
}
