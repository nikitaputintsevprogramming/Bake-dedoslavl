using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Bake
{
    public class TimeDetector : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private float _endTime;

        [SerializeField] private float _currTime;
        [SerializeField] private float _reserveTime;

        //[SerializeField] private Text textTestTime;
        [SerializeField] private Slider sliderTestTime;

        private void Start()
        {
            sliderTestTime.value = _endTime;
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
            //Debug.LogFormat("������� ����� � �������� �����:{0}", _currTime);
            if (_currTime >= _endTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return true;
            }
            return false;
        }

        private void ResetTimer()
        {
            _reserveTime += _currTime;
        }

        public void TextShowResetTime(Text textTestTime)
        {
            textTestTime.text = "������� �����: " + sliderTestTime.value.ToString("0") + " ������";
        }

        public void OnValueChangedResetSlider()
        {
            _endTime = sliderTestTime.value;
            ResetTimer();
        }
    }
}


