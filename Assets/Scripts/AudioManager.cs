using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI.Pagination
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PagedRect _pageRect;

        [SerializeField] private AudioSource _backgroundAudio;
        [SerializeField] private AudioSource _mysteryAudio;
        [SerializeField] private AudioClip[] mysteries;

        private void Start()
        {
            _backgroundAudio.Stop();
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

                // Проверка, что длина строки достаточно велика, чтобы взять подстроку начиная с пятого символа
                if (input.Length >= 6)
                {
                    if (int.TryParse(input.Substring(5, 1), out int numResult))
                    {
                        Debug.LogFormat("Вы выбрали трек №: {0}", numResult);

                        _mysteryAudio.clip = mysteries[numResult - 1];
                        _mysteryAudio.Play();
                        while(_backgroundAudio.volume > 0.1f)
                        {
                            _backgroundAudio.volume = Mathf.Lerp(_backgroundAudio.volume, 0.1f, Time.deltaTime);
                        }
                    }
                }

            }
        }
    }
}

