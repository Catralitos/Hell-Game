using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Events.ScriptableObjects.UI;
using Gameplay.ScriptableObjects;
using Inputs;
using Interaction;
using Menu;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scene UI")] public MenuSelectionHandler selectionHandler;
        //public UIPopup popupPanel;
        public UIDialogueManager dialogueController;
        //public UIInteraction interactionPanel;
        //public UIPause pauseScreen;

        [Header("Gameplay")] public GameStateSO gameStateManager;
        //public MenuSO mainMenu;
        public InputReader inputReader;
        public ActorSO mainProtagonist;

        [Header("Listening on")] 
        public VoidEventChannelSO onSceneReady;

        [Header("Dialogue Events")] 
        public DialogueLineChannelSO openUIDialogueEvent;
        public IntEventChannelSO closeUIDialogueEvent;
        
        [Header("Interaction Events")] [SerializeField]
        public InteractionUIEventChannelSO setInteractionEvent;

        //[Header("Broadcasting on ")] [SerializeField]
        //private LoadEventChannelSO loadMenuEvent;

        [Header("Broadcasting on ")]
        public VoidEventChannelSO onInteractionEndedEvent;

        private void OnEnable()
        {
            onSceneReady.OnEventRaised += ResetUI;
            openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
            closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
            inputReader.MenuPauseEvent +=
                OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open
            setInteractionEvent.OnEventRaised += SetInteractionPanel;
        }

        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= ResetUI;
            openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
            closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;
            inputReader.MenuPauseEvent -= OpenUIPause;
            setInteractionEvent.OnEventRaised -= SetInteractionPanel;
        }

        private void ResetUI()
        {
            dialogueController.gameObject.SetActive(false);
            /*inventoryPanel.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        interactionPanel.gameObject.SetActive(false);*/

            Time.timeScale = 1;
        }

        private void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
        {
            bool isProtagonistTalking = actor == mainProtagonist;
            dialogueController.SetDialogue(dialogueLine, actor, isProtagonistTalking);
            //interactionPanel.gameObject.SetActive(false);
            dialogueController.gameObject.SetActive(true);
        }

        private void CloseUIDialogue(int dialogueType)
        {
            selectionHandler.Unselect();
            dialogueController.gameObject.SetActive(false);
            onInteractionEndedEvent.RaiseEvent();
        }

        public void OpenUIPause()
        {
            inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

            Time.timeScale = 0; // Pause time

            /*pauseScreen.BackToMainRequested +=
            ShowBackToMenuConfirmationPopup; //once the UI Pause popup is open, listen to back to menu button
        pauseScreen.Resumed += CloseUIPause; //once the UI Pause popup is open, listen to unpause event

        pauseScreen.gameObject.SetActive(true);*/

            inputReader.EnableMenuInput();
            gameStateManager.UpdateGameState(GameState.Pause);
        }

        private void CloseUIPause()
        {
            Time.timeScale = 1; // unpause time

            inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

            // once the popup is closed, you can't listen to the following events 
            /*pauseScreen.BackToMainRequested -=
            ShowBackToMenuConfirmationPopup; //once the UI Pause popup is open, listen to back to menu button
        pauseScreen.Resumed -= CloseUIPause; //once the UI Pause popup is open, listen to unpause event

        pauseScreen.gameObject.SetActive(false);*/

            gameStateManager.ResetToPreviousGameState();

            if (gameStateManager.currentGameState == GameState.Gameplay)
            {
                inputReader.EnableGameplayInput();
            }

            selectionHandler.Unselect();
        }


        private void ShowBackToMenuConfirmationPopup()
        {
            //pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

            /*popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;

            popupPanel.ConfirmationResponseAction += BackToMainMenu;

            inputReader.EnableMenuInput();
            popupPanel.gameObject.SetActive(true);
            popupPanel.SetPopup(PopupType.BackToMenu);*/
        }

        private void BackToMainMenu(bool confirm)
        {
            HideBackToMenuConfirmationPopup(); // hide confirmation screen, show close UI pause, 

            if (confirm)
            {
                CloseUIPause(); //close ui pause to unsub from all events 
                //loadMenuEvent.RaiseEvent(mainMenu, false); //load main menu
            }
        }

        private void HideBackToMenuConfirmationPopup()
        {
           /* popupPanel.ClosePopupAction -= HideBackToMenuConfirmationPopup;
            popupPanel.ConfirmationResponseAction -= BackToMainMenu;

            popupPanel.gameObject.SetActive(false);*/
            selectionHandler.Unselect();
            //pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

            // time is still set to 0 and Input is still set to menuInput 
            //going out from confirmaiton popup screen gets us back to the pause screen
        }

        private void SetInventoryScreen()
        {
            if (gameStateManager.currentGameState == GameState.Gameplay)
            {
                OpenInventoryScreen();
            }
        }

        private void OpenInventoryScreen()
        {
            inputReader.MenuPauseEvent -= OpenUIPause; // player cant open the UI Pause again when they are in inventory  
            inputReader.MenuUnpauseEvent -= CloseUIPause; // player can close the UI Pause popup when they are in inventory 

            inputReader.MenuCloseEvent += CloseInventoryScreen;
            inputReader.CloseRadialMenuEvent += CloseInventoryScreen;

            //inventoryPanel.FillInventory();

            //inventoryPanel.gameObject.SetActive(true);
            inputReader.EnableMenuInput();

            gameStateManager.UpdateGameState(GameState.Inventory);
        }

        private void CloseInventoryScreen()
        {
            inputReader.MenuPauseEvent += OpenUIPause; // you cant open the UI Pause again when you are in inventory  

            inputReader.MenuCloseEvent -= CloseInventoryScreen;
            inputReader.CloseRadialMenuEvent -= CloseInventoryScreen;

            //inventoryPanel.gameObject.SetActive(false);

            selectionHandler.Unselect();
            gameStateManager.ResetToPreviousGameState();
            if (gameStateManager.currentGameState == GameState.Gameplay)
                inputReader.EnableGameplayInput();
        }

        private void SetInteractionPanel(bool isOpen, InteractionType interactionType)
        {

            if (isOpen)
            {
                // interactionPanel.FillInteractionPanel(interactionType);
            }

            //interactionPanel.gameObject.SetActive(isOpen);
        
        }
    }
}

