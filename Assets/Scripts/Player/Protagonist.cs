using System;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
	/// <summary>
	/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerInput))]
	public class Protagonist : MonoBehaviour
	{
		public InputReader inputReader;

		private Vector2 _moveVector;
		private Vector2 _aimVector;

		/// <summary>
		/// The run speed
		/// </summary>
		[Header("Movement Variables")] public float runSpeed = 20.0f;

		/// <summary>
		/// The item the player is holding
		/// </summary>
		[Header("Attached Objects")] public GameObject itemHolder;
	
		/// <summary>
		/// The body
		/// </summary>
		private Rigidbody2D _body;
		/// <summary>
		/// The player input
		/// </summary>
		private PlayerInput _input;
        
		/// <summary>
		/// The current angle of the character
		/// </summary>
		private float _angle;
		/// <summary>
		/// The angle in the last frame
		/// </summary>
		private float _lastAngle;
	
		//These fields are read and manipulated by the StateMachine actions
		[NonSerialized] public bool useItemInput;
		[NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
		[NonSerialized] public ControllerColliderHit lastHit;
		[NonSerialized] public bool isAiming; // Used when using the keyboard to run, brings the normalised speed to 1

		private void Start()
		{
			_body = GetComponent<Rigidbody2D>();
			_input = GetComponent<PlayerInput>();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			lastHit = hit;
		}

		//Adds listeners for events being triggered in the InputReader script
		private void OnEnable()
		{
			inputReader.EnableGameplayInput();
			inputReader.MoveEvent += onMove;
			inputReader.AimEvent += onAim;
			inputReader.AimStartedEvent += OnStartedAiming;
			inputReader.AimCanceledEvent += OnStoppedAiming;
			inputReader.UseItemEvent += OnStartedUseItem;
		}

		//Removes all listeners to the events coming from the InputReader script
		private void OnDisable()
		{
			inputReader.MoveEvent -= onMove;
			inputReader.AimEvent -= onAim;
			inputReader.AimStartedEvent -= OnStartedAiming;
			inputReader.AimCanceledEvent -= OnStoppedAiming;
			inputReader.UseItemEvent -= OnStartedUseItem;
		}

		private void Update()
		{
			if (isAiming)
			{
				AimWeapon();
			}
			else if (_moveVector != Vector2.zero)
			{
				_lastAngle = _angle;
				_angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
				itemHolder.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
			}
		}

		private void FixedUpdate()
		{
			if (isAiming)
			{
				_body.velocity = Vector2.zero;
				return;
			}
			_body.velocity =_moveVector * runSpeed;
		}
	
		private void AimWeapon()
		{
			
			//If the player is using a controller, and the stick is not neutral
			//I'm using magnitude to check, because stick never goes beyond 1.
			//WASD is usually one or higher
			Debug.Log(_input.currentControlScheme);
			if (_input.currentControlScheme == "Controller" && _moveVector != Vector2.zero)
			{
				_lastAngle = _angle;
				_angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
				if (Math.Abs(_lastAngle - _angle) > inputReader.controllerRotationSensitivity)
				{
					itemHolder.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
				}
			} 
		    else if (_input.currentControlScheme == "Keyboard")
			{
				if (Camera.main != null)
				{
					Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.localPosition);

					Vector3 mousePosition = (_aimVector - (Vector2) screenPosition).normalized;
                    
					_lastAngle = _angle;
					_angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
                    
					if (Math.Abs(_lastAngle - _angle) > inputReader.mouseRotationSensitivity)
					{
						itemHolder.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
					}
				}
			}
		}

	

		//---- EVENT LISTENERS ----

		private void onMove(Vector2 movement)
		{
			_moveVector = movement;
		}
	
		private void onAim(Vector2 aiming)
		{
			_aimVector = aiming;
		}
		
		private void OnStoppedAiming() => isAiming = false;

		private void OnStartedAiming() => isAiming = true;
	

		private void OnStartedUseItem() => useItemInput = true;

		// Triggered from Animation Event
		public void ConsumeUseItemInput() => useItemInput = false;
	}
}
