using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;



namespace TopDown.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : Movement
    {
       private void OnMove(InputValue value)
        {
         
            Vector3 playerInput = new Vector3(value.Get<Vector2>().x, value.Get<Vector2>().y, 0);
            curentInput = playerInput;
        }
    }
}
