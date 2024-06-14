using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pagination
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PagedRect _pageRect;

        private void Update()
        {
            CheckSensor();
            GetCurrentScene();
        }

        public bool CheckSensor()
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                Debug.LogFormat("Датчик сработал");
                return true;
            }
            return false;
        }

        public bool GetCurrentScene()
        {
            if (_pageRect.GetCurrentPage().PageNumber == 1)
            {
                Debug.LogFormat("Датчик сработал, показ костра, текущая сцена: {0}", _pageRect.GetCurrentPage().PageNumber);
                
                return true;
            }
            return false;
        }
    }
}

