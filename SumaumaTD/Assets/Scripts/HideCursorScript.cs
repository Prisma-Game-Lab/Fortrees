using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class HideCursorScript : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetJoystickNames().Length > 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
