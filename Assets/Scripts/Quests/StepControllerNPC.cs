using Characters;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Gameplay.ScriptableObjects;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Quests
{
    public class StepControllerNPC : MonoBehaviour
    {
        public bool isInDialogue; //Consumed by the state machine
	
        [Header("Data")]
        public ActorSO actor;
        public DialogueDataSO defaultDialogue;
        public QuestManagerSO questData;
        public GameStateSO gameStateManager;
		
        [Header("Listening to channels")]
        public VoidEventChannelSO winDialogueEvent;
        public VoidEventChannelSO loseDialogueEvent;
        public IntEventChannelSO endDialogueEvent;
    
        [Header("Broadcasting on channels")]
        public DialogueDataChannelSO startDialogueEvent;
    
        //check if character is actif. An actif character is the character concerned by the step.
        private DialogueDataSO _currentDialogue;
    
        private void PlayDefaultDialogue()
        {

            if (defaultDialogue != null)
            {
                _currentDialogue = defaultDialogue;
                StartDialogue();
            }
        }
    
        //start a dialogue when interaction
        //some Steps need to be instantaneous. And do not need the interact button.
        //when interaction again, restart same dialogue.
        public bool InteractWithCharacter()
        {
            if (gameStateManager.currentGameState == GameState.Gameplay)
            {
                DialogueDataSO displayDialogue = questData.InteractWithCharacter(actor, false, false);
                //Debug.Log("dialogue " + displayDialogue + "actor" + actor);
                if (displayDialogue != null)
                {
                    _currentDialogue = displayDialogue;
                    StartDialogue();
                    return true;
                }
                 PlayDefaultDialogue();
                return true;
            }

            return false;
        }

        private void StartDialogue()
        {
            startDialogueEvent.RaiseEvent(_currentDialogue);
            endDialogueEvent.OnEventRaised += EndDialogue;
            StopConversation();
            winDialogueEvent.OnEventRaised += PlayWinDialogue;
            loseDialogueEvent.OnEventRaised += PlayLoseDialogue;
            isInDialogue = true;
        }

        private void EndDialogue(int dialogueType)
        {
            endDialogueEvent.OnEventRaised -= EndDialogue;
            winDialogueEvent.OnEventRaised -= PlayWinDialogue;
            loseDialogueEvent.OnEventRaised -= PlayLoseDialogue;
            ResumeConversation();
            isInDialogue = false;
        }

        private void PlayLoseDialogue()
        {
            if (questData != null)
            {
                DialogueDataSO displayDialogue = questData.InteractWithCharacter(actor, true, false);
                if (displayDialogue != null)
                {
                    _currentDialogue = displayDialogue;
                    StartDialogue();
                }
            }
        }

        private void PlayWinDialogue()
        {
            if (questData != null)
            {
                DialogueDataSO displayDialogue = questData.InteractWithCharacter(actor, true, true);
                if (displayDialogue != null)
                {
                    _currentDialogue = displayDialogue;
                    StartDialogue();
                }
            }
        }

        private void StopConversation()
        {
            GameObject[] talkingTo = gameObject.GetComponent<NPC>().talkingTo;
            if (talkingTo != null)
            {
                foreach (GameObject t in talkingTo)
                {
                    t.GetComponent<NPC>().npcState = NPCState.Idle;
                }
            }
        }

        private void ResumeConversation()
        {
            GameObject[] talkingTo = GetComponent<NPC>().talkingTo;
            if (talkingTo != null)
            {
                foreach (GameObject t in talkingTo)
                {
                    t.GetComponent<NPC>().npcState = NPCState.Talk;
                }
            }
        }
    }
}