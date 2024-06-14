using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pagination
{
    public class InputManager : MonoBehaviour//, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] private PagedRect _pageRect;
        
        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    Debug.Log("OnPointerClick");
        //}

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    Debug.Log("OnPointerDown");
        //}

        private void Update()
        {
            CheckSensor();
            SetFireScene();
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

        public bool SetFireScene()
        {
            if (Input.GetKey(KeyCode.Tab) && _pageRect.GetCurrentPage().PageNumber == 1)
            {
                Debug.LogFormat("Датчик сработал, показ костра, текущая сцена: {0}", _pageRect.GetCurrentPage().PageNumber);
                
                return true;
            }
            return false;
        }
    }
}

