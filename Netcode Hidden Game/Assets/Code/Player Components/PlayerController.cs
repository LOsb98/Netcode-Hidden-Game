using Mirror;
using UnityEngine;
using HiddenGame.Misc;

namespace HiddenGame.PlayerComponents
{
    // TODO : Implement input system package for controls
    public abstract class PlayerController : NetworkBehaviour
    {
        #region Movement states
        [SerializeField] private Vector3 _move;
        private bool _jumping;
        private bool _grounded;

        #endregion

        #region Mouse input values
        [SerializeField] private float _mouseSens = 1f;
        private float _verticalRotation = 0f;
        #endregion

        #region Ground check values
        [SerializeField] private float _slopeCheckDistance;
        [SerializeField] private Transform _groundCheckPos;
        [SerializeField] private float _groundCheckSize;
        [SerializeField] private LayerMask _groundCheckLayer;

        #endregion

        #region  Component references
        [SerializeField] private Rigidbody _rb;
        [SerializeField] protected Transform _camPos;
        [SerializeField] protected Movement _movement;

        [SerializeField] protected CreateNetworkHitscanRay _networkHitscanRaycast;
        [SerializeField] protected CreateNetworkProjectile _networkProjectile;

        public Transform CamPos => _camPos;

        #endregion

        [SerializeField] protected float _abilityCooldown;
        protected float _abilityCooldownTimer;

        protected abstract void FireAbility();

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        protected void Start()
        {
            if (isLocalPlayer)
            {
                //Having one camera in scene which tracks the local player is better than manually destroying every non-player camera
                //More flexibility in the long run with telling the camera where to go
                CameraFollow.Instance.enabled = true;
                CameraFollow.Instance.FollowPos = _camPos;
            }
        }

        protected void Update()
        {
            if (!isLocalPlayer)
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

            CheckIfGrounded();
            GetMovementInput();
            GetMouseInput();

            CheckJumpInput();

            if (_abilityCooldownTimer > 0)
            {
                _abilityCooldownTimer -= Time.deltaTime;
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                FireAbility();
                _abilityCooldownTimer = _abilityCooldown;
            }
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            //Take movement inputs in Update()
            //Apply them in FixedUpdate for consistency
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
        }

        private void CheckIfGrounded()
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
        }

        private Vector3 CheckSlope() 
        { 
            //Check for slope
            RaycastHit slopeCheck;

            if (Physics.Raycast(transform.position, Vector3.down, out slopeCheck, _slopeCheckDistance, _groundCheckLayer))
            {
                ////If on flat surface
                //if (slopeCheck.normal == Vector3.up)
                //{
                //    //Drag the player down while they are on a flat surface
                //    //Stops them from flying up when walking up a slope onto a flat surface
                //    //Or vice versa
                //    _move += new Vector3(0, -_slopeForce, 0);
                //    return;
                //}



        
                //Debug.Log($"X: {slopeX}");
                //Debug.Log($"Y: {slopeY}");
                //Debug.Log($"Z: {slopeZ}");
            }
            Debug.Log($"This slope: {slopeCheck.normal}");
            return slopeCheck.normal;
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
            _movement.HorizontalMove(_move, CheckSlope(), _grounded);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckSize);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * _slopeCheckDistance));
        }
    }
}
