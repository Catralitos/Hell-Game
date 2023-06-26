﻿using Dialogues.ScriptableObjects;
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
        [Tooltip("How many angels to kill if it's a kill enemy step")]
        public int angelCount;
        [Tooltip("Which angels to kill if it's a kill enemy step")]
        public int angelBatch;
        public bool isDone;
        [Tooltip("Specific event which happens when step is done")]
        public VoidEventChannelSO endStepEvent;

        
        public void FinishStep()
        {
            if (endStepEvent != null)
                endStepEvent.RaiseEvent();
            isDone = true;
        }
    }
}