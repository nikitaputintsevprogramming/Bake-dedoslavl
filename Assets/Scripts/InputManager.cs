using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pagination
{
    public class InputManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] private PagedRect _pageRect;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                _pageRect.SetCurrentPage(2);
                Debug.Log("Датчик сработал, показ костра");
            }
        }
    }
}

