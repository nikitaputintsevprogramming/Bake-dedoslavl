using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pagination
{
    public class InputManager : MonoBehaviour
    {
        private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

        private void Update()
        {
            CheckSensor();
        }

        //Датчик
        public bool CheckSensor()
        {
            if (Input.GetKey(KeyCode.Tab))
            {
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
                        //для теста на пк
                        if(keyCode != KeyCode.Mouse0)
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

