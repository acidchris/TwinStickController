
using Game.Damaging.Scripts;
using Game.Player.Scripts;
using Game.Shooting.Scripts;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class TwinStickMovement : MonoBehaviour
    {
        [SerializeField] private float _playerSpeed = 5f;
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _controllerDeadzone = 0.1f;
        [SerializeField] private float _gamepadRotateSmoothing = 1000f;
        [SerializeField] private float _keyboardMouseRotationSmoothing = 10f;

        private bool _isGamepad = false;
        private CharacterController _characterController = null;

        private Vector2 _movement = Vector2.zero;
        private Vector2 _aim = Vector2.zero;
        private Vector2 _mouseAim = Vector2.zero;
        private bool _mouseShootingStarted = false;

        private Vector3 _targetPosition;

        private Vector3 _playerVelocity = Vector3.zero;

        private PlayerControls _playerControls = null;
        private PlayerInput _playerInput = null;

        private Animator _animator = null;

        private IShootingInstigator _shootingInstigator = null;
        private PlayerDamageReceiver _playerDamageReceiver = null;

        private int _animForwardHash = Animator.StringToHash("runningForward");
        private int _animBackwardHash = Animator.StringToHash("runningBackward");

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _playerControls = new PlayerControls();
            _playerInput.onControlsChanged += OnInputDeviceChanged;

            _animator = GetComponentInChildren<Animator>();

            _shootingInstigator = GetComponent<IShootingInstigator>();
            _playerDamageReceiver = GetComponentInChildren<PlayerDamageReceiver>();
        }

        protected void Start()
        {
            _playerDamageReceiver._onDied += OnDied;
        }

        private void OnKeyboardMouseShootStart(InputAction.CallbackContext obj)
        {
            _mouseShootingStarted = true;
        }

        private void OnKeyboardMouseShootCancelled(InputAction.CallbackContext obj)
        {
            _mouseShootingStarted = false;
        }

        protected void OnDestroy()
        {
            _playerInput.onControlsChanged -= OnInputDeviceChanged;

            _playerDamageReceiver._onDied -= OnDied;
        }

        private void OnEnable()
        {
            _playerControls.Enable();

            _playerControls.Controls.Shoot.started += OnKeyboardMouseShootStart;
            _playerControls.Controls.Shoot.canceled += OnKeyboardMouseShootCancelled;
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            _playerControls.Controls.Shoot.performed -= OnKeyboardMouseShootStart;
            _playerControls.Controls.Shoot.canceled -= OnKeyboardMouseShootCancelled;
            _mouseShootingStarted = false;
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
        }

        private void HandleRotation()
        {
            if (_isGamepad)
            {
                if (Mathf.Abs(_aim.x) > _controllerDeadzone || Mathf.Abs(_aim.y) > _controllerDeadzone)
                {
                    Vector3 playerDirection = Vector3.right * _aim.x + Vector3.forward * _aim.y;

                    if (playerDirection.sqrMagnitude > 0.01f)
                    {
                        Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, _gamepadRotateSmoothing * Time.deltaTime);
                    }

                    //anticipate a target position?
                    Vector3 targetPos = (transform.forward * 100.0f + transform.position);
                    Debug.DrawLine(transform.position, (transform.forward * 100.0f + transform.position), Color.green, 1f);

                    //fire a shot
                    _shootingInstigator.DoShoot(targetPos);
                }
            }
            else
            {
                if (_mouseAim != Vector2.zero)
                {
                    //smooth AF rotation if using mouse
                    Ray ray = Camera.main.ScreenPointToRay(_mouseAim);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                        Quaternion rotation = Quaternion.LookRotation(_targetPosition - transform.position);

                        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _keyboardMouseRotationSmoothing);
                    }
                }

                //workaround for continuous "hold mouse shoot button" inputs
                if (_mouseShootingStarted)
                {
                    _shootingInstigator.DoShoot(_targetPosition);
                }
            }
        }

        private void HandleMovement()
        {
            Vector3 moveAmount = new Vector3(_movement.x, 0, _movement.y);

            _characterController.Move(moveAmount * Time.deltaTime * _playerSpeed);

            _playerVelocity.y += _gravityValue * Time.deltaTime;

            _characterController.Move(_playerVelocity * Time.deltaTime);

            UpdateAnimation(moveAmount);
        }

        private void HandleInput()
        {
            _movement = _playerControls.Controls.Movement.ReadValue<Vector2>();

            //need to differentiate between two different types. Stick input values messed everything up because connected
            if (_isGamepad)
            {
                _aim = _playerControls.Controls.Aim.ReadValue<Vector2>();
            }
            else
            {
                _mouseAim = _playerControls.Controls.MouseAim.ReadValue<Vector2>();
            }
        }

        //update to anim blending one day :)
        private void UpdateAnimation(Vector3 moveAmount)
        {
            if (moveAmount == Vector3.zero)
            {
                //idleing
                _animator.SetBool(_animForwardHash, false);
                _animator.SetBool(_animBackwardHash, false);
            }
            else
            {
                //positive means running in the direction the player is viewing
                float forwardDir = Vector3.Dot(transform.forward, moveAmount);

                if (forwardDir > 0.1f)
                {
                    _animator.SetBool(_animForwardHash, true);
                    _animator.SetBool(_animBackwardHash, false);
                }
                else
                {
                    _animator.SetBool(_animForwardHash, false);
                    _animator.SetBool(_animBackwardHash, true);
                }
            }
        }

        private void OnInputDeviceChanged(PlayerInput playerInput)
        {
            _isGamepad = playerInput.currentControlScheme.Contains("Gamepad");
        }

        private void OnDied(object sender, HealthChangeInfo _)
        {
            _playerDamageReceiver._onDied -= OnDied;

            //disable all kinds of player inputs
            _playerInput.enabled = false;
            _playerControls.Disable();
            _mouseShootingStarted = false;

            _animator.SetTrigger("died");
        }

    }

}
