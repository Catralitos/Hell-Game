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
        }

        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= ResetUI;
            openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
            closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;
        }

        private void ResetUI()
        {
            dialogueController.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        private void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
        {
            bool isProtagonistTalking = actor == mainProtagonist;
            dialogueController.SetDialogue(dialogueLine, actor, isProtagonistTalking);
            //interactionPanel.gameObject.SetActive(false);
            dialogueController.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void CloseUIDialogue(int dialogueType)
        {
            Time.timeScale = 1;
            selectionHandler.Unselect();
            dialogueController.gameObject.SetActive(false);
            onInteractionEndedEvent.RaiseEvent();
        }
    }
}

