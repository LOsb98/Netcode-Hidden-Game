using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.ScriptableObjects;

namespace HiddenGame.PlayerComponents
{
    //This class handles applying the actual movement and holds values like movement speed
    //Controller scripts decide when to perform actions, check states, and calculate movement vectors
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _slopeForce;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpStartHeight;
        [SerializeField] private Rigidbody _rb;

        private void OnValidate()
        {
            if (!_rb)
            {
                if (TryGetComponent(out Rigidbody rb))
                {
                    _rb = rb;
                }
                else
                {
                    Debug.LogError($"{gameObject} uses movement but doesn't have a rigidbody component attached");
                }
            }
        }

        public void SetData(CharacterData charData)
        {
            _speed = charData.Speed;
            _jumpForce = charData.JumpForce;
            
        }

        public void HorizontalMove(Vector3 moveVector, Vector3 slopeNormal, bool isGrounded)
        {
            if (isGrounded)
            {
                //Account for slope angle
                Vector3 slopeVector = (Vector3.Cross(slopeNormal, transform.right)) * -1f;
                Debug.DrawRay(transform.position, slopeVector, Color.green);

                Vector3 slopeMovementDirection = Vector3.Cross(slopeNormal, moveVector);

                Vector3 finalMoveDirection = Vector3.Cross(slopeMovementDirection, slopeNormal);

                moveVector = finalMoveDirection.normalized;
                Debug.DrawRay(transform.position, finalMoveDirection, Color.blue);
            }

            moveVector *= _speed;

            Debug.Log(moveVector);

            if (!isGrounded)
            {
                moveVector = new Vector3(moveVector.x, _rb.velocity.y, moveVector.z);
            }

            if (slopeNormal == Vector3.up)
            {
                //Check if player is near a flat surface
                //If they are, drag them down to stop them flying off slopes

                moveVector = new Vector3(moveVector.x, -_slopeForce, moveVector.z);
            }

            _rb.velocity = moveVector;
        }

        public void AirMove(Vector3 moveVector, float maxSpeed)
        {
            if (_rb.velocity.magnitude <= maxSpeed)
            {
                _rb.AddForce(moveVector, ForceMode.Acceleration);
            }

            float maxAxisSpeed = maxSpeed / 2;

            //Clamping velocity is needed when using AddForce()
            //Stops player gaining infinite speed
            _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -maxAxisSpeed, maxAxisSpeed), _rb.velocity.y, Mathf.Clamp(_rb.velocity.z, -maxAxisSpeed, maxAxisSpeed));
        }

        public void Jump()
        {
            transform.position += new Vector3(0f, _jumpStartHeight, 0f);
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
        }
    }
}
