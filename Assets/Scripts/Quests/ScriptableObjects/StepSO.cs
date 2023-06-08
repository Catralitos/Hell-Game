using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Quests.ScriptableObjects
{

    public enum StepType
    {
        Dialogue,
        GiveItem,
        CheckItem,
        KillEnemy
    }
    
    [CreateAssetMenu(menuName = "Quests/Step")]
    public class StepSO : ScriptableObject
    {
        
        [Tooltip("The Character this step will need interaction with")]
        public ActorSO actor;
        [Tooltip("The dialogue that will be displayed before an action, if any")]
        public DialogueDataSO dialogueBeforeStep;
        [Tooltip("The dialogue that will be displayed when the step is achieved")]
        public DialogueDataSO completeDialogue;
        [Tooltip("The dialogue that will be displayed if the step is not achieved yet")]
        public DialogueDataSO incompleteDialogue ;
        public StepType type; 
        [Tooltip("The item to check/give")]
        public ItemSO item;
        public bool hasReward;
        [Tooltip("The item to reward if any")]
        public ItemSO rewardItem;
        public bool isDone;
        public VoidEventChannelSO endStepEvent;

        
        public void FinishStep()
        {
            if (endStepEvent != null)
                endStepEvent.RaiseEvent();
            isDone = true;
        }
    }
}