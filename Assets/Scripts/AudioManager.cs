using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Pagination
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        //[SerializeField] private TimeDetector _timeDetector;
        [SerializeField] private PagedRect _pageRect;

        [SerializeField] private AudioSource _backgroundAudio;
        [SerializeField] private AudioSource[] mysteries;

        private void Start()
        {
            _backgroundAudio.Stop();
        }

        private void Update()
        {
            if (_inputManager.SetFireScene())
            {
                Debug.Log("_backgroundAudio.Play");
                _backgroundAudio.Play();
                _pageRect.SetCurrentPage(2);
            }
        }
    }
}

