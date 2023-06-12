using System.Collections.Generic;
using Events.ScriptableObjects;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class QuestSO : ScriptableObject
    {
        public string questName;
        public bool isDone;
        public ItemSO reward;
        public List<StepSO> steps;

        public VoidEventChannelSO endQuestEvent;

        [HideInInspector] public StepSO currentStep;
        [HideInInspector] public int currentStepIndex; 
        
        public void FinishQuest()
        {
            isDone = true;
            if(endQuestEvent != null)
            {
                endQuestEvent.RaiseEvent(); 
            }
        }
    }
}
