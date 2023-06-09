using System;
using Gameplay.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Inputs
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader :  ScriptableObject, PlayerControls.IGameplayActions, PlayerControls.IDialoguesActions, PlayerControls.IMenusActions
    {
        [Space]
        public GameStateSO gameStateManager;
	
        // Assign delegate{} to events to initialise them with an empty delegate
        // so we can skip the null check when we use them
	
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
    
        // Gameplay
        public event UnityAction UseItemEvent = delegate { };
        public event UnityAction UseItemCanceledEvent = delegate { };
        public event UnityAction InteractEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction<Vector2> AimEvent = delegate { };
        public event UnityAction AimStartedEvent = delegate { };
        public event UnityAction AimCanceledEvent = delegate { };
        public event UnityAction OpenRadialMenuEvent = delegate { }; // Used to bring up the inventory
        public event UnityAction CloseRadialMenuEvent = delegate { }; // Used to bring up the inventory
    
        // Shared between menus and dialogues
        public event UnityAction MoveSelectionEvent = delegate { };

        // Dialogues
        public event UnityAction AdvanceDialogueEvent = delegate { };

        // Menus
        public event UnityAction MenuMouseMoveEvent = delegate { };
        public event UnityAction MenuClickButtonEvent = delegate { };
        public event UnityAction MenuCloseEvent = delegate { };
        public event UnityAction MenuUnpauseEvent = delegate { };
        public event UnityAction MenuPauseEvent = delegate { };

        private PlayerControls _gameInput;
    
        public void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new PlayerControls();

                _gameInput.Menus.SetCallbacks(this);
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.Dialogues.SetCallbacks(this);
            }
        }

        public void OnDisable()
        {
            DisableAllInput();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //MoveEvent.Invoke(context.ReadValue<Vector2>());  
            if (context.control.device.name == "Controller")
            {
                var value = context.ReadValue<Vector2>();
                var valueRounded = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
                if (valueRounded.magnitude > controllerPushingSensitivity)
                {
                    MoveEvent.Invoke(valueRounded);
                }
            }
            else
            {
                MoveEvent.Invoke(context.ReadValue<Vector2>());  
            }
        }
        
        public void OnAim(InputAction.CallbackContext context)
        {
            AimEvent.Invoke(context.ReadValue<Vector2>());
        }
	
        public void OnStopToAim(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    AimStartedEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    AimCanceledEvent.Invoke();
                    break;
            }
        }
	
        public void OnUseItem(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    UseItemEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    UseItemCanceledEvent.Invoke();
                    break;
            }    
        }
	
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed
                && gameStateManager.currentGameState == GameState.Gameplay) // Interaction is only possible when in gameplay GameState
                InteractEvent.Invoke();
        }
    
        public void OnRadialMenu(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    OpenRadialMenuEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    CloseRadialMenuEvent.Invoke();
                    break;
            }
        
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuPauseEvent.Invoke();
        }
    
        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MoveSelectionEvent.Invoke();
        }

        public void OnAdvanceDialogue(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Performed)
                AdvanceDialogueEvent.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuClickButtonEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuCloseEvent.Invoke();
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuMouseMoveEvent.Invoke();
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuUnpauseEvent.Invoke();
        }
    
        public void EnableDialogueInput()
        {
            _gameInput.Menus.Enable();
            _gameInput.Gameplay.Disable();
            _gameInput.Dialogues.Enable();
        }

        public void EnableGameplayInput()
        {
            _gameInput.Menus.Disable();
            _gameInput.Dialogues.Disable();
            _gameInput.Gameplay.Enable();
        }

        public void EnableMenuInput()
        {
            _gameInput.Dialogues.Disable();
            _gameInput.Gameplay.Disable();

            _gameInput.Menus.Enable();
        }

        public void DisableAllInput()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Menus.Disable();
            _gameInput.Dialogues.Disable();
        }
    
        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
    
        public void OnSubmit(InputAction.CallbackContext context)
        {

        }

        public void OnNavigate(InputAction.CallbackContext context)
        {

        }
    
    }
}