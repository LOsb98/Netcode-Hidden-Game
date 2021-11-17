using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float xVector;
    [SerializeField] private float yVector;
    [SerializeField] private float zVector;

    #region Movement values
    public bool _grounded;
    public float _speed;
    public int _jumpForce;
    private bool _jumping;
    public Vector3 _move;
    #endregion

    # region Mouse input values
    public float _mouseSens;
    private float _verticalRotation = 0f;
    #endregion

    #region Ground check values
    [SerializeField] private float _slopeCheckDistance;
    [SerializeField] private float _slopeForce;
    public Transform _groundCheckPos;
    public float _groundCheckSize;
    public LayerMask _groundCheckLayer;

    #endregion

    #region  Component references
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _camPos;
    [SerializeField] private Movement _movement;

    public Transform CamPos => _camPos;

    #endregion

    private void Start()
    {
        if (!IsLocalPlayer)
        {
            Destroy(this);
            return;
        }

        //Having one camera in scene which tracks the local player is better than manually destroying every non-player camera
        //More flexibility in the long run with telling the camera where to go
        CameraFollow.Instance.FollowPos = _camPos;
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }


        GetMovementInput();
        GetMouseInput();
        CheckGround();

        CheckJumpInput();
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        ApplyMovementInput();
        if (_jumping)
        {
            _movement.Jump();
            _jumping = false;
        }
    }

    private void GetMouseInput()
    {
        //Taking mouse input values to rotate player body
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens;
        transform.Rotate(0, mouseX, 0);

        //Vertical camera rotation, clamped to 90 degrees up and down
        _verticalRotation -= Input.GetAxis("Mouse Y") * _mouseSens;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        _camPos.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void GetMovementInput()
    {
        //Movement keys
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        //Taking right and forward axis and multiplying them by movement input values
        //Gives Vector3 to use for moving player
        _move = (transform.right * xAxis + transform.forward * zAxis);
        _move.Normalize();
        _move *= _speed;
    }

    private void CheckGround()
    {
        //Ground check sphere
        if (!Physics.CheckSphere(_groundCheckPos.position, _groundCheckSize, _groundCheckLayer))
        {
            _rb.useGravity = true;

            _grounded = false;
        }
        else
        {
            //When touching the ground, turn off gravity to stop sliding down slopes
            _rb.useGravity = false;
            _grounded = true;
        }

        //Check for slope
        RaycastHit slopeCheck;

        if (Physics.Raycast(transform.position, Vector3.down, out slopeCheck, _slopeCheckDistance))
        {
            if (slopeCheck.normal == Vector3.up)
            {
                //Return if the surface is not a slope
                //Also drag the player down while they are on a flat surface
                //Stops them from flying up when walking up a slope onto a flat surface
                _rb.velocity = new Vector3(_rb.velocity.x, -5f, _rb.velocity.z);
                return;
            }

            Vector3 slopeVector = (Vector3.Cross(slopeCheck.normal, transform.right)) * -1f;

            Debug.DrawRay(transform.position, slopeVector, Color.green);

            Vector3 slopeMovement = Vector3.Cross(slopeCheck.normal, _move);
            Vector3 finalMove = Vector3.Cross(slopeMovement, slopeCheck.normal);

            _move = finalMove.normalized * _speed;

            Debug.DrawRay(transform.position, finalMove, Color.blue);

            //float slopeX = slopeCheck.normal.x;
            //float slopeY = slopeCheck.normal.y;
            //float slopeZ = slopeCheck.normal.z;

            //Debug.Log($"This slope: {slopeCheck.normal}");
            //Debug.Log($"X: {slopeX}");
            //Debug.Log($"Y: {slopeY}");
            //Debug.Log($"Z: {slopeZ}");
        }
    }

    private void CheckJumpInput()
    {
        //Jumping
        if (Input.GetKeyDown("space") && _grounded == true)
        {
            _jumping = true;
        }
    }

    private void ApplyMovementInput()
    {
        //Using different methods for aerial and grounded movement
        if (!_grounded)
        {
            //_movement.AirMove(_rb, _move, _speed);
            return;
        }
        _movement.GroundMove(_move);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckSize);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * _slopeCheckDistance));

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(xVector, yVector, zVector));
    }
}
