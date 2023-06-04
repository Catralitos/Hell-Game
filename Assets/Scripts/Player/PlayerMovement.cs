using System;
using UnityEngine;

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
        /// The item the player is holding
        /// </summary>
        [Header("Attached Objects")] public GameObject heldItem;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator _animator;
        /// <summary>
        /// The body
        /// </summary>
        private Rigidbody2D _body;
        
        /// <summary>
        /// The current angle of the character
        /// </summary>
        private float _angle;
        /// <summary>
        /// The angle in the last frame
        /// </summary>
        private float _lastAngle;

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
            if (PlayerEntity.Instance.menuOpen) return;
            
            if (PlayerEntity.Instance.aiming)
            {
                AimWeapon();
            }
            else if (PlayerEntity.Instance.move != Vector2.zero)
            {
                _lastAngle = _angle;
                _angle = Mathf.Atan2(PlayerEntity.Instance.move.y, PlayerEntity.Instance.move.x) * Mathf.Rad2Deg;
                heldItem.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            }
        }

        /// <summary>
        /// Updates this instance at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            if (PlayerEntity.Instance.aiming || PlayerEntity.Instance.menuOpen)
            {
                _body.velocity = Vector2.zero;
                return;
            }
            _body.velocity = PlayerEntity.Instance.move * runSpeed;
        }

        /// <summary>
        /// Rotates to the right position
        /// </summary>
        private void AimWeapon()
        {
            //If the player is using a controller, and the stick is not neutral
            if (PlayerEntity.Instance.input.currentControlScheme == "Gamepad" && PlayerEntity.Instance.move != Vector2.zero)
            {
                _lastAngle = _angle;
                _angle = Mathf.Atan2(PlayerEntity.Instance.move.y, PlayerEntity.Instance.move.x) * Mathf.Rad2Deg;
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

                    Vector3 mousePosition = (PlayerEntity.Instance.aim - (Vector2) screenPosition).normalized;
                    
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