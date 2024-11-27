using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace TopDown.Movement
{


    public class PlayerRotation : Rotator
    {
        private void OnLook(InputValue value)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
            LookAt(mousePosition);
        }
    }
}
