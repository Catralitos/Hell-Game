using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// The class that handles the player movement
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerMovement : MonoBehaviour
    {

        /// <summary>
        /// The mouse rotation sensitivity
        /// </summary>
        [Header("Input Sensitivity")] public float mouseRotationSensitivity = -1.15f;
        /// <summary>
        /// The controller movement sensitivity
        /// </summary>
        public float controllerPushingSensitivity = 0.75f;
        /// <summary>
        /// The controller rotation sensitivity
        /// </summary>
        public float controllerRotationSensitivity = 1.15f;

        /// <summary>
        /// The run speed
        /// </summary>
        [Header("Movement Variables")] public float runSpeed = 20.0f;
        /// <summary>
        /// The angle offset of the character
        /// </summary>
        public int angleOffset = -90;

        /// <summary>
        /// The item the player is holding
        /// </summary>
        [Header("Attached Objects")] public GameObject heldItem;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator _animator;
        /// <summary>
        /// The player controls
        /// </summary>
        private PlayerControls _playerControls;
        /// <summary>
        /// The body
        /// </summary>
        private Rigidbody2D _body;
        
        /// <summary>
        /// If the character is stopped to aim
        /// </summary>
        private bool _aiming;
        /// <summary>
        /// If the character has the menu open
        /// </summary>
        private bool _menuOpen;
        /// <summary>
        /// The current angle of the character
        /// </summary>
        private float _angle;
        /// <summary>
        /// The angle in the last frame
        /// </summary>
        private float _lastAngle;
        
        /// <summary>
        /// The mouse aim direction
        /// </summary>
        private Vector2 _aim;
        /// <summary>
        /// The move direction
        /// </summary>
        private Vector2 _move;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            //_gameManager = GameManager.Instance;
            _playerControls = new PlayerControls();
            //mouseControl = _gameManager.mouseControls;
            //Using the next input system, depending on the controls, we set up the various events
            
            _playerControls.Gameplay.Move.performed += ctx => {
                if (PlayerEntity.Instance.input.currentControlScheme == "Controller")
                {
                    var value = ctx.ReadValue<Vector2>();
                    var valueRounded = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
                    if (valueRounded.magnitude < controllerPushingSensitivity) return;
                    _move = valueRounded;
                }
                else
                {
                    _move = ctx.ReadValue<Vector2>();
                }
            };
            _playerControls.Gameplay.Move.canceled += _ => { _move = Vector2.zero; };
            _playerControls.Gameplay.Aim.performed += ctx => { 
                if (PlayerEntity.Instance.input.currentControlScheme == "Controller")
                {
                    var value = ctx.ReadValue<Vector2>();
                    var valueRounded = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
                    if (valueRounded.magnitude < controllerPushingSensitivity) return;
                    _aim = valueRounded;
                }
                else
                {
                    _aim = ctx.ReadValue<Vector2>();
                }
            };
            _playerControls.Gameplay.Aim.canceled += _ => { _aim = Vector2.zero; };
            _playerControls.Gameplay.UseItem.performed += _ => { };
            _playerControls.Gameplay.UseItem.performed += _ => { };
            _playerControls.Gameplay.Interact.performed += _ => { };
            _playerControls.Gameplay.Interact.performed += _ => { };
            _playerControls.Gameplay.StopToAim.performed += _ => { _aiming = true; };
            _playerControls.Gameplay.StopToAim.canceled += _ => { _aiming = false; };
            _playerControls.Gameplay.RadialMenu.performed += _ => { _menuOpen = true; };
            _playerControls.Gameplay.RadialMenu.canceled += _ => { _menuOpen = false; };

        }

        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
            _playerControls.Gameplay.Enable();
        }

        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            _playerControls.Gameplay.Disable();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            if (_menuOpen) return;
            
            if (_aiming)
            {
                AimWeapon();
            }
            else
            {
                _lastAngle = _angle;
                _angle = Mathf.Atan2(_move.y, _move.x) * Mathf.Rad2Deg;
                heldItem.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            }
        }

        /// <summary>
        /// Updates this instance at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            if (_menuOpen) return;
            if (_aiming)
            {
                _body.velocity = Vector2.zero;
                return;
            }
            _body.velocity = _move * runSpeed;
        }

        /// <summary>
        /// Rotates to the right position
        /// </summary>
        private void AimWeapon()
        {
            //If the player is using a controller, and the stick is not neutral
            if (PlayerEntity.Instance.input.currentControlScheme == "Gamepad" && _move != Vector2.zero)
            {
                _lastAngle = _angle;
                _angle = Mathf.Atan2(_move.y, _move.x) * Mathf.Rad2Deg;
                if (Math.Abs(_lastAngle - _angle) > controllerRotationSensitivity)
                {
                    heldItem.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                }
            } 
            else 
            {
                if (Camera.main != null)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.localPosition);

                    Vector3 mousePosition = (_aim - (Vector2) screenPosition).normalized;
                    
                    _lastAngle = _angle;
                    _angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
                    
                    if (Math.Abs(_lastAngle - _angle) > mouseRotationSensitivity)
                    {
                        heldItem.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    }
                }
            }
        }
    }
}