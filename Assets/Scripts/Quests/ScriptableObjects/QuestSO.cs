using System.Collections.Generic;
using Events.ScriptableObjects;
using Extensions;
using Inventory.ScriptableObjects;
using Management;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class QuestSO : ScriptableObject
    {
        public Pair<TimeStep, TimeStep> availableWindow;
        public string questName;
        public bool isDone;
        public ItemSO reward;
        public List<StepSO> steps;

        public VoidEventChannelSO endQuestEvent;

        [HideInInspector] public StepSO currentStep;
        [HideInInspector] public int currentStepIndex;

        public bool QuestIsAvailable(TimeStep time)
        {
              return availableWindow.FirstMember.Compare(time) <= 0 && availableWindow.SecondMember.Compare(time) >= 0;
        }
        
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
