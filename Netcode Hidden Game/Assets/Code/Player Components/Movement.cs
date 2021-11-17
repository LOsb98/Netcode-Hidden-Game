using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenGame.PlayerComponents
{
    public class Movement : MonoBehaviour
    {
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

        public void GroundMove(Vector3 moveVector)
        {
            //Setting velocity directly makes ground movement more consistent
            //Reduces how many physics factors can affect movement
            _rb.velocity = new Vector3(moveVector.x, moveVector.y, moveVector.z);
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
