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
        //[SerializeField] private float acceleration;
        private Rigidbody2D body;

        protected Vector3 curentInput;


        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            body.velocity = movementSpeed * curentInput * Time.fixedDeltaTime;
        }
    }

}