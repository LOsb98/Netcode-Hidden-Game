using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Movement values
    public bool _grounded;
    public float _speed;
    public int _jumpForce;
    public Vector3 _move;
    #endregion

    # region Mouse input values
    public float _mouseSens;
    private float _verticalRotation = 0f;
    #endregion

    #region Ground check values
    public Transform _groundCheckPos;
    public float _groundCheckSize;
    public LayerMask _groundCheckLayer;
    #endregion

    #region  Component references
    private Rigidbody _rb;
    public Movement _movement;
    public Transform _camPos;
    #endregion

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        #region mouse
        //Movement keys
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        //Taking right and forward axis and multiplying them by movement input values
        //Gives Vector3 to use for moving player
        _move = (transform.right * xAxis + transform.forward * zAxis);
        _move.Normalize();
        _move *= _speed;

        //Taking mouse input values to rotate player body
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens;
        transform.Rotate(0, mouseX, 0);

        //Vertical camera rotation, clamped to 90 degrees up and down
        _verticalRotation -= Input.GetAxis("Mouse Y") * _mouseSens;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        _camPos.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        #endregion


        #region controls and movement
        //Ground check sphere
        if (!Physics.CheckSphere(_groundCheckPos.position, _groundCheckSize, _groundCheckLayer)) _grounded = false;
        else _grounded = true;
        
        //Jumping
        if (Input.GetKeyDown("space") && _grounded == true)
        {
            _movement.Jump(_rb, _jumpForce);
        }

        //Using different methods for aerial and grounded movement
        if (!_grounded)
        {
            _movement.AirMove(_rb, _move, _speed);
            return;
        }
        _movement.GroundMove(_rb, _move, _speed);
        #endregion
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckSize);
    }
}
