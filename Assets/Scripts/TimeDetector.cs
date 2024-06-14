using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Pagination
{
    public class TimeDetector : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private float _endTime;

        public float _currTime;
        public float _reserveTime;

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
            //_currTime = _reserveTime - _currTime;
        }
    }
}


