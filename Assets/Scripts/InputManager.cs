using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bake
{
    public class InputManager : MonoBehaviour
    {
        private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
        [SerializeField] private Button btn;

        public void ShowTestPanel(GameObject Panel) 
        {
            if (!Input.GetKeyDown(KeyCode.Return))
                Panel.SetActive(!Panel.activeInHierarchy);
        }

        private void Update()
        {
            CheckSensor();
        }

        public void EnterEnter()
        {
            if(!Input.GetKeyDown(KeyCode.Return))
            btn.onClick.Invoke();
        }

        //Датчик
        public bool CheckSensor()
        {
            if (Input.GetKey(KeyCode.Return))
            {
                EnterEnter();
                Debug.LogFormat("Датчик сработал");
                return true;
            }
            return false;
        }


        public KeyCode CheckKeys()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in keyCodes)
                {
                    if (Input.GetKey(keyCode))
                    {
                        if (keyCode != KeyCode.Mouse0)
                        {
                            Debug.Log("KeyCode down: " + keyCode);
                            return keyCode;
                        }
                    }
                }
            }
            return KeyCode.None;
        }
    }
}

