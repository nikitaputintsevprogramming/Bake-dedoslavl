using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UI.Pagination; //Фрейм
using System.Linq;
using UnityEngine.UI;
using System;

namespace Bake
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PagedRect _pageRect;

        [SerializeField] private AudioSource _backgroundAudio;
        [SerializeField] private AudioSource _mysteryAudio;
        public List<AudioClip> mysteries;

        [SerializeField] private float _maxVolume = 0.5f;
        [SerializeField] private float _minVolume = 0.1f;
        [SerializeField] private float duration = 2.0f;

        [SerializeField] private Slider _sliderMinValue;
        [SerializeField] private Slider _sliderMaxValue;

        private void Start()
        {
            _backgroundAudio.Stop();
            _backgroundAudio.volume = _maxVolume;

            if (PlayerPrefs.GetFloat("MaxVol") != 0)
                _maxVolume = PlayerPrefs.GetFloat("MaxVol");
            _sliderMaxValue.value = PlayerPrefs.GetFloat("MaxVol");

            if (PlayerPrefs.GetFloat("MinVol") != 0)
                _minVolume = PlayerPrefs.GetFloat("MinVol");
            _sliderMinValue.value = PlayerPrefs.GetFloat("MinVol");
        }

        private void Update()
        {
            if (_pageRect.GetCurrentPage().PageNumber == 1 && _inputManager.CheckSensor())
            {
                Debug.Log("_backgroundAudio.Play");
                _backgroundAudio.Play();
                _pageRect.SetCurrentPage(2);
            }

            if (_pageRect.GetCurrentPage().PageNumber == 2)
            {
                //int NumberTrack = Int32.Parse(_inputManager.CheckKeys().ToString().Substring(5, 1));
                string input = _inputManager.CheckKeys().ToString();
                Debug.Log(input.Length);
                // Проверка, что длина строки достаточно велика, чтобы взять подстроку начиная с пятого символа
                if (input.Length >= 7)
                {
                    Debug.Log("input.Length >= 7");
                    if (int.TryParse(input.Substring(6, 1), out int numResult))
                    {
                        if (numResult <= mysteries.Count - 1)
                        {
                            Debug.LogFormat("Вы выбрали трек №: {0}", numResult);

                            _mysteryAudio.clip = mysteries[numResult];
                            _mysteryAudio.Play();
                            StartCoroutine(FadeVolume(_mysteryAudio, _minVolume, _maxVolume, duration));

                            //_backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, 0.1f, 10 * Time.deltaTime);
                            //_backgroundAudio.volume = 0.1f;
                            StartCoroutine(FadeVolume(_backgroundAudio, _maxVolume, _minVolume, duration));
                        }
                    }
                }
            }

            if(!_mysteryAudio.isPlaying && _backgroundAudio.volume <=_minVolume)
            {
                StartCoroutine(FadeVolume(_backgroundAudio, _minVolume, _maxVolume, duration));
            }
        }

        IEnumerator FadeVolume(AudioSource audioSource, float startVolume, float targetVolume, float duration)
        {
            float _startVolume = startVolume;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(_startVolume, targetVolume, time / duration);
                yield return null;
            }

            audioSource.volume = targetVolume;
        }

        public void ChangeMinVol(Text textTarget)
        {
            textTarget.text = "Мин. громкость при затухании: " + (_sliderMinValue.value * 100).ToString("0");
            PlayerPrefs.SetFloat("MinVol", _sliderMinValue.value);
            _minVolume = _sliderMinValue.value;
        }

        public void ChangeMaxVol(Text textTarget)
        {
            textTarget.text = "Макс. громкость при затухании: " + (_sliderMaxValue.value * 100).ToString("0");
            PlayerPrefs.SetFloat("MaxVol", _sliderMaxValue.value);
            _maxVolume = _sliderMaxValue.value;
        }
    }
}

