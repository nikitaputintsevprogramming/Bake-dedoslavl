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

        private Dictionary<KeyCode, int> keyToTrackIndex = new Dictionary<KeyCode, int>
        {
            { KeyCode.Keypad0, 0 }, // 0
            { KeyCode.Keypad1, 1 }, // 1
            { KeyCode.Keypad2, 2 }, // 2
            { KeyCode.Keypad3, 3 }, // 3
            { KeyCode.Keypad4, 4 }, // 4
            { KeyCode.Keypad5, 5 }, // 5
            { KeyCode.Keypad6, 6 }, // 6
            { KeyCode.Keypad7, 7 }, // 7
            { KeyCode.Keypad8, 8 }, // 8
            { KeyCode.Keypad9, 9 }, // 9
            //{ KeyCode.Clear, 10 }, // clear
            { KeyCode.KeypadDivide, 10 }, // / (знак деления)
            { KeyCode.KeypadMultiply, 11 }, // * (знак умножения)
            { KeyCode.Backspace, 12 }, // Удалить
            { KeyCode.KeypadMinus, 13 }, // -
            { KeyCode.KeypadPlus, 14 }, // +
            //{ KeyCode.KeypadEnter, 15 }, // NumpadEnter
            { KeyCode.KeypadPeriod, 15 } // . (точка)
        };

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

        List<AudioClip> SortByNumberInName(List<AudioClip> clips)
        {
            return clips.OrderBy(clip => ExtractNumberFromName(clip.name)).ToList();
        }

        int ExtractNumberFromName(string name)
        {
            var digits = new string(name.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int number) ? number : 0;
        }

        private void Update()
        {
            if (_pageRect.GetCurrentPage().PageNumber == 1 && _inputManager.CheckSensor())
            {
                _backgroundAudio.Play();
                _pageRect.SetCurrentPage(2);
            }

            if (_pageRect.GetCurrentPage().PageNumber == 2)
            {
                mysteries = SortByNumberInName(mysteries);

                KeyCode input = _inputManager.CheckKeys();
                if (keyToTrackIndex.ContainsKey(input))
                {
                    int trackIndex = keyToTrackIndex[input];
                    Debug.Log(trackIndex); // Проверка
                    if (trackIndex < mysteries.Count)
                    {
                        Debug.LogFormat("Вы выбрали трек №: {0}", trackIndex);

                        _mysteryAudio.clip = mysteries[trackIndex];
                        _mysteryAudio.Play();
                        StartCoroutine(FadeVolume(_mysteryAudio, _minVolume, _maxVolume, duration));

                        StartCoroutine(FadeVolume(_backgroundAudio, _maxVolume, _minVolume, duration));
                    }
                }
            }

            if (!_mysteryAudio.isPlaying && _backgroundAudio.volume <= _minVolume)
            {
                StartCoroutine(FadeVolume(_backgroundAudio, _minVolume, _maxVolume, duration));
            }
        }

        public IEnumerator FadeVolume(AudioSource audioSource, float startVolume, float targetVolume, float duration)
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

