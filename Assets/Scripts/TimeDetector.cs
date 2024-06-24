using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Pagination; //Фрейм
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bake
{
    public class TimeDetector : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private PagedRect _pageRect;

        [SerializeField] private float _endTime;

        [SerializeField] private float _currTime;
        [SerializeField] private float _reserveTime;

        //[SerializeField] private Text textTestTime;
        [SerializeField] private Slider sliderTestTime;
        [SerializeField] private Text textTimeSec;

        private void Start()
        {
            if(PlayerPrefs.GetFloat("TimeReset") == 0)
                sliderTestTime.value = _endTime;
            sliderTestTime.value = PlayerPrefs.GetFloat("TimeReset");
            Debug.Log("PlayerPrefs.GetFloat(TimeReset)" + PlayerPrefs.GetFloat("TimeReset"));
            textTimeSec.text = "Рестарт через: " + PlayerPrefs.GetFloat("TimeReset").ToString("0") + " секунд";
        }

        private void Update()
        {
            Timer();
            if(_inputManager.CheckSensor())
            {
                ResetTimer();
            }
        }

        public bool Timer()
        {
            _currTime = Time.timeSinceLevelLoad - _reserveTime;
            //Debug.LogFormat("Текущее время с загрузки сцены:{0}", _currTime);
            if (_currTime >= _endTime)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                _pageRect.SetCurrentPage(1);
                DisableAllAudioSources();
                return true;
            }
            return false;
        }

        public void DisableAllAudioSources()
        {
            AudioSource[] _audioSources = FindObjectsOfType<AudioSource>();
            foreach(AudioSource source in _audioSources)
            {
                //source.Stop();
                StartCoroutine(_audioManager.FadeVolume(source, source.volume, 0, 2f));
            }
        }

        private void ResetTimer()
        {
            _reserveTime += _currTime;
        }

        public void TextShowResetTime(Text textTestTime)
        {
            textTestTime.text = "Рестарт через: " + sliderTestTime.value.ToString("0") + " секунд";
        }

        public void OnValueChangedResetSlider()
        {
            _endTime = sliderTestTime.value;
            PlayerPrefs.SetFloat("TimeReset", sliderTestTime.value);
            Debug.Log("sliderTestTime.value for set PlayerPrefs" + sliderTestTime.value);
            Debug.Log("PlayerPrefs.GetFloat(TimeReset)" + PlayerPrefs.GetFloat("TimeReset"));
            ResetTimer();
        }
    }
}


