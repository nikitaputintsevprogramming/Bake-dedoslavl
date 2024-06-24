using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UI.Pagination; //Фрейм
using System.Linq;
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

        [SerializeField] private float _endVolume = 0.5f;
        [SerializeField] private float _startVolume = 0.1f;
        [SerializeField] private float duration = 2.0f;

        private void Start()
        {
            _backgroundAudio.Stop();
            _backgroundAudio.volume = _endVolume;
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
                            StartCoroutine(FadeVolume(_mysteryAudio, _startVolume, _endVolume, duration));

                            //_backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, 0.1f, 10 * Time.deltaTime);
                            //_backgroundAudio.volume = 0.1f;
                            StartCoroutine(FadeVolume(_backgroundAudio, _endVolume, _startVolume, duration));
                        }
                    }
                }
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
    }
}

