using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Singleton to easily access all the player's components
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerEntity : MonoBehaviour
    {

        /// <summary>
        /// The PlayerMovement
        /// </summary>
        [HideInInspector] public PlayerInput input;
        /// <summary>
        /// The PlayerMovement
        /// </summary>
        [HideInInspector] public PlayerMovement movement;
        /// <summary>
        /// The PlayerHealth
        /// </summary>
        [HideInInspector] public PlayerHealth health;
        /// <summary>
        /// The PlayerRanged
        /// </summary>
        [HideInInspector] public PlayerRanged ranged;
        /// <summary>
        /// The PlayerMelee
        /// </summary>
        [HideInInspector] public PlayerMelee melee;
        
        /// <summary>
        /// The player controls
        /// </summary>
        private PlayerControls _playerControls;
        
        /// <summary>
        /// If the character is stopped to aim
        /// </summary>
        [HideInInspector] public bool aiming;
        /// <summary>
        /// If the character has the menu open
        /// </summary>
        [HideInInspector] public bool menuOpen;
        
        /// <summary>
        /// The mouse aim direction
        /// </summary>
        [HideInInspector] public Vector2 aim;
        /// <summary>
        /// The move direction
        /// </summary>
        [HideInInspector] public Vector2 move;
        
        
        #region SingleTon

        /// <summary>
        /// Gets the sole instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PlayerEntity Instance { get; private set; }

        /// <summary>
        /// Awakes this instance (if none exist).
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                input = GetComponent<PlayerInput>();
                movement = GetComponent<PlayerMovement>();
                health = GetComponent<PlayerHealth>();
                ranged = GetComponent<PlayerRanged>();
                melee = GetComponent<PlayerMelee>();
                
                _playerControls = new PlayerControls();

                _playerControls.Gameplay.Move.performed += ctx => {
                    if (input.currentControlScheme == "Controller")
                    {
                        var value = ctx.ReadValue<Vector2>();
                        var valueRounded = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
                        if (valueRounded.magnitude < movement.controllerPushingSensitivity) return;
                        move = valueRounded;
                    }
                    else
                    {
                        move = ctx.ReadValue<Vector2>();
                    }
                };
                _playerControls.Gameplay.Move.canceled += _ => { move = Vector2.zero; };
                _playerControls.Gameplay.Aim.performed += ctx => { 
                    if (input.currentControlScheme == "Controller")
                    {
                        var value = ctx.ReadValue<Vector2>();
                        var valueRounded = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
                        if (valueRounded.magnitude < movement.controllerPushingSensitivity) return;
                        aim = valueRounded;
                    }
                    else
                    {
                        aim = ctx.ReadValue<Vector2>();
                    }
                };
                _playerControls.Gameplay.Aim.canceled += _ => { aim = Vector2.zero; };
                _playerControls.Gameplay.UseItem.performed += _ => { };
                _playerControls.Gameplay.Interact.performed += _ => { };
                _playerControls.Gameplay.StopToAim.performed += _ => { aiming = true; };
                _playerControls.Gameplay.StopToAim.canceled += _ => { aiming = false; };
                _playerControls.Gameplay.RadialMenu.performed += _ => { menuOpen = true; };
                _playerControls.Gameplay.RadialMenu.canceled += _ => { menuOpen = false; };
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
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
    }
}