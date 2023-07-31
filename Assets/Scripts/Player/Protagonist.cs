using System;
using Gameplay.ScriptableObjects;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

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
		public GameObject inventoryMenu;
		
		/// <summary>
		/// The body
		/// </summary>
		private Rigidbody2D _body;
		/// <summary>
		/// The player input
		/// </summary>
		private PlayerInput _input;

		private Animator _animator;
        
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

		//private bool _menuOpen;
		private static readonly int Horizontal = Animator.StringToHash("Horizontal");
		private static readonly int Vertical = Animator.StringToHash("Vertical");
		private static readonly int Speed = Animator.StringToHash("Speed");

		private void Start()
		{
			_body = GetComponent<Rigidbody2D>();
			_input = GetComponent<PlayerInput>();
			_animator = GetComponent<Animator>();
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
			//inputReader.OpenRadialMenuEvent += OpenMenu;
			//inputReader.CloseRadialMenuEvent += CloseMenu;
		}

		//Removes all listeners to the events coming from the InputReader script
		private void OnDisable()
		{
			inputReader.MoveEvent -= onMove;
			inputReader.AimEvent -= onAim;
			inputReader.AimStartedEvent -= OnStartedAiming;
			inputReader.AimCanceledEvent -= OnStoppedAiming;
			//inputReader.OpenRadialMenuEvent -= OpenMenu;
			//inputReader.CloseRadialMenuEvent -= CloseMenu;
		}

		private void Update()
		{
			if (inventoryMenu.activeSelf || inputReader.gameStateManager.currentGameState != GameState.Gameplay || _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeal"))
			{
				_animator.SetFloat(Speed, 0.0f);
				return;
			}
			if (isAiming)
			{
				AimWeapon();
			}
			_animator.SetFloat(Horizontal, _moveVector.x);
			_animator.SetFloat(Vertical, _moveVector.y);
            _animator.SetFloat(Speed, _moveVector.sqrMagnitude);
			if (_moveVector != Vector2.zero)
			{
				_lastAngle = _angle;
				_angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
				itemHolder.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
				int y = itemHolder.transform.rotation.z is > 90 or < -90 ? -1 : 1;
				itemHolder.transform.localScale = new Vector3(1, y, 1);
			}
		}

		private void FixedUpdate()
		{
			if (isAiming || inventoryMenu.activeSelf || inputReader.gameStateManager.currentGameState != GameState.Gameplay || _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeal"))
			{
				_body.velocity = Vector2.zero;
				return;
			}
			_body.velocity =_moveVector * runSpeed;
		}
	
		private void AimWeapon()
		{
			switch (_input.currentControlScheme)
			{
				//If the player is using a controller, and the stick is not neutral
				//I'm using magnitude to check, because stick never goes beyond 1.
				//WASD is usually one or higher
				case "Controller" when _moveVector != Vector2.zero:
				{
					_lastAngle = _angle;
					_angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
					if (Math.Abs(_lastAngle - _angle) > inputReader.controllerRotationSensitivity)
					{
						itemHolder.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
					}
					int y = itemHolder.transform.rotation.z is > 90 or < -90 ? -1 : 1;
					itemHolder.transform.localScale = new Vector3(1, y, 1);
					break;
				}
				case "Keyboard":
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
						int y = itemHolder.transform.rotation.z is > 90 or < -90 ? -1 : 1;
						itemHolder.transform.localScale = new Vector3(1, y, 1);
					}
					break;
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
		
		private void OnStartedAiming() => isAiming = true;

		private void OnStoppedAiming() => isAiming = false;
		
		/*private void OpenMenu()
		{
			if (inputReader.gameStateManager.currentGameState == GameState.Gameplay)
			{
				_menuOpen = true;
			}
		}

		private void CloseMenu()
		{
			if (inputReader.gameStateManager.currentGameState == GameState.Gameplay)
			{
				_menuOpen = false;
			}
		}*/
	}
}
