using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UIElements;

namespace TopDown.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deaceleration;

        private Rigidbody2D body;
        private Vector2 currentSpeed;
        protected Vector2 curentInput;


        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            currentSpeed = Vector3.zero;
        }
        private void FixedUpdate()
        {
            if (curentInput == Vector2.zero)
            {
                if (body.velocity.sqrMagnitude > 0.01f)
                {
                    //finds the mid point between current speed and a full stop. Then slowly decelerates 
                    body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, deaceleration * Time.fixedDeltaTime);
                }
                else
                {
                    body.velocity = Vector2.zero;  //if close to zero, stops
                }
            }
            else
            {
                if (curentInput != body.velocity.normalized)
                {
                    body.velocity = Vector2.Lerp(body.velocity, movementSpeed * curentInput * Time.fixedDeltaTime, acceleration/1.5f * Time.fixedDeltaTime);
                }
                else
                {

                    body.velocity = Vector2.Lerp(body.velocity, movementSpeed * curentInput * Time.fixedDeltaTime, acceleration * Time.fixedDeltaTime);
                }
            }
        }
    }

}