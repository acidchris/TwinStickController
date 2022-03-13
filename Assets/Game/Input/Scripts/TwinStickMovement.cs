
using Game.Shooting.Scripts;
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

        private bool _isGamepad = false;
        private CharacterController _characterController = null;

        private Vector2 _movement;
        private Vector2 _aim;

        private Vector3 _playerVelocity = Vector3.zero;

        private PlayerControls _playerControls = null;
        private PlayerInput _playerInput = null;

        private Animator _animator = null;

        private IShootingInstigator _shootingInstigator = null;

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _playerControls = new PlayerControls();
            _playerInput.onControlsChanged += OnInputDeviceChanged;

            _animator = GetComponentInChildren<Animator>();

            _shootingInstigator = GetComponent<IShootingInstigator>();
        }

        protected void OnDestroy()
        {
            _playerInput.onControlsChanged -= OnInputDeviceChanged;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
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

                    //alternative, less precise though
                    //transform.LookAt(new Vector3(_aim.x + transform.position.x, 0f, _aim.y + transform.position.z));
                    //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


                    //fire a shot
                    _shootingInstigator.DoShoot(1f);
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(_aim);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 point = ray.GetPoint(rayDistance);
                    LookAt(point);
                }
            }

        }

        private void LookAt(Vector3 lookPoint)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);

            transform.LookAt(heightCorrectedPoint);
        }

        private void HandleMovement()
        {
            Vector3 moveAmount = new Vector3(_movement.x, 0, _movement.y);

            if (moveAmount == Vector3.zero)
            {
                //idleing
                _animator.SetBool("running", false);
            }
            else
            {
                //running
                _animator.SetBool("running", true);
            }

            _characterController.Move(moveAmount * Time.deltaTime * _playerSpeed);

            _playerVelocity.y += _gravityValue * Time.deltaTime;

            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void HandleInput()
        {
            _movement = _playerControls.Controls.Movement.ReadValue<Vector2>();
            _aim = _playerControls.Controls.Aim.ReadValue<Vector2>();
        }

        public void OnInputDeviceChanged(PlayerInput playerInput)
        {
            _isGamepad = playerInput.currentControlScheme.Contains("Gamepad");
        }
    }

}
