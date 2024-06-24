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
        [SerializeField] private Text textTimeSec;

        private void Start()
        {
            if(PlayerPrefs.GetFloat("TimeReset") == 0)
                sliderTestTime.value = _endTime;
            sliderTestTime.value = PlayerPrefs.GetFloat("TimeReset");
            Debug.Log("PlayerPrefs.GetFloat(TimeReset)" + PlayerPrefs.GetFloat("TimeReset"));
            textTimeSec.text = "������� �����: " + PlayerPrefs.GetFloat("TimeReset").ToString("0") + " ������";
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
            PlayerPrefs.SetFloat("TimeReset", sliderTestTime.value);
            Debug.Log("sliderTestTime.value for set PlayerPrefs" + sliderTestTime.value);
            Debug.Log("PlayerPrefs.GetFloat(TimeReset)" + PlayerPrefs.GetFloat("TimeReset"));
            ResetTimer();
        }
    }
}


