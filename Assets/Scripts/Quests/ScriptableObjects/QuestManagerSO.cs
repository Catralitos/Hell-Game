using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Events.ScriptableObjects.UI;
using Inventory.ScriptableObjects;
using Management.ScriptableObjects;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Quest Manager")]
    public class QuestManagerSO : ScriptableObject
    {
        [Header("Data")]
        public List<QuestSO> quests;
        public InventorySO inventory;
        public EnemyTrackerSO enemyTracker;
        /*[Tooltip("Item the NPC gives out on winning")]
        public ItemSO winningItem;
        [Tooltip("Item the NPC gives out on losing")]
        public ItemSO losingItem;*/

        [Header("Listening to channels")]
        public DialogueChoiceChannelSO continueWithStepEvent;
        public IntEventChannelSO endDialogueEvent;
        public DialogueActorChannelSO makeWinningChoiceEvent;
        public DialogueActorChannelSO makeLosingChoiceEvent;

        [Header("Broadcasting on channels")]
        public VoidEventChannelSO playCompletionDialogueEvent;
        public VoidEventChannelSO playIncompleteDialogueEvent;
        public ItemEventChannelSO giveItemEvent;
        public ItemSOEventChannelSO rewardItemSoEvent;
        //public SaveSystem saveSystem;

        private StepSO _lastStepChecked;

        public void Init()
        {
            foreach (QuestSO q in quests)
            {
                foreach (StepSO s in q.steps)
                {
                    s.isDone = false;
                }

                q.currentStepIndex = 0;
                q.currentStep = q.steps[0];
                q.isDone = false;
            }

            _lastStepChecked = null;
        }

        public void OnDisable()
        {
            continueWithStepEvent.OnEventRaised -= CheckStepValidity;
            endDialogueEvent.OnEventRaised -= EndDialogue;
            makeWinningChoiceEvent.OnEventRaised -= MakeWinningChoice;
            makeLosingChoiceEvent.OnEventRaised -= MakeLosingChoice;
        }


        public void OnEnable()
        {
            continueWithStepEvent.OnEventRaised += CheckStepValidity;
            endDialogueEvent.OnEventRaised += EndDialogue;
            makeWinningChoiceEvent.OnEventRaised += MakeWinningChoice;
            makeLosingChoiceEvent.OnEventRaised += MakeLosingChoice;
        }

        private StepSO HasStep(ActorSO actorToCheckWith)
        {
            return (from quest in quests where !quest.isDone && quest.currentStep.actor == actorToCheckWith select quest.currentStep).FirstOrDefault();
        }

        private QuestSO GetStepQuest(StepSO step)
        {
            return quests.FirstOrDefault(quest => !quest.isDone && quest.steps.Contains(step));
        }

        private StepSO GetStepWithChoice(Choice choice)
        {
            if (_lastStepChecked != null)
            {
                if (_lastStepChecked.dialogueBeforeStep.lines.Any(l => l.choices.Contains(choice)))
                {
                    return _lastStepChecked;
                }
                if (_lastStepChecked.completeDialogue != null)
                {
                    if (_lastStepChecked.completeDialogue.lines.Any(l => l.choices.Contains(choice)))
                    {
                        return _lastStepChecked;
                    }
                }
                if (_lastStepChecked.incompleteDialogue != null)
                {
                    if (_lastStepChecked.incompleteDialogue.lines.Any(l => l.choices.Contains(choice)))
                    {
                        return _lastStepChecked;
                    }
                }
            }
            foreach (var step in quests.Where(q => !q.isDone).SelectMany(q => q.steps))
            {
                if (step.dialogueBeforeStep.lines.Any(l => l.choices.Contains(choice)))
                {
                    return step;
                }
                if (step.completeDialogue != null)
                {
                    if (_lastStepChecked.completeDialogue.lines.Any(l => l.choices.Contains(choice)))
                    {
                        return step;
                    }
                }
                if (step.incompleteDialogue != null)
                {
                    if (_lastStepChecked.incompleteDialogue.lines.Any(l => l.choices.Contains(choice)))
                    {
                        return step;
                    }
                }
            }

            return null;
        }

        public DialogueDataSO InteractWithCharacter(ActorSO actor, bool isCheckValidity, bool isValid)
        {
            StepSO currentStep = HasStep(actor);
            if (currentStep != null)
            {
                _lastStepChecked = currentStep;
                if (isCheckValidity)
                {
                    return isValid ? currentStep.completeDialogue : currentStep.incompleteDialogue;
                }
                return currentStep.dialogueBeforeStep;
            }
            return null;
        }


        private void MakeWinningChoice(ActorSO actor)
        {
            StepSO currentStep = HasStep(actor);
            if (currentStep != null)
            {
                _lastStepChecked = currentStep;
                //currentStep.item = winningItem;
                CheckStepValidity(currentStep);
            }
        }

        private void MakeLosingChoice(ActorSO actor)
        {
            StepSO currentStep = HasStep(actor);
            if (currentStep != null)
            {
                _lastStepChecked = currentStep;
                //currentStep.item = losingItem;
                CheckStepValidity(currentStep);
            }
        }

        // um evento que passa algo. que podemos passar?
        //este evento é raised no dialogue manager em:
        //MakeDialogueChoice, que tem parametro choice
        private void CheckStepValidity(Choice currentChoice)
        {
            StepSO currentStep = GetStepWithChoice(currentChoice);
            if (currentStep != null)
            {
                switch (currentStep.type)
                {
                    case StepType.CheckItem:
                        if (inventory.Contains(currentStep.item))
                        {
                            //Trigger win dialogue
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }

                        break;

                    case StepType.GiveItem:
                        if (inventory.Contains(currentStep.item))
                        {
                            giveItemEvent.RaiseEvent(inventory.GetKeyItem(currentStep.item));
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }

                        break;

                    case StepType.Dialogue:
                        //dialogue has already been played
                        if (currentStep.completeDialogue != null)
                        {
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            EndStep(currentStep);
                        }

                        break;
                    case StepType.KillEnemy:
                        if (enemyTracker.GetNumAngels(currentStep.angelBatch) >= currentStep.angelCount)
                        {
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void CheckStepValidity(StepSO currentStep)
        {
            if (currentStep != null)
            {
                switch (currentStep.type)
                {
                    case StepType.CheckItem:
                        if (inventory.Contains(currentStep.item))
                        {
                            //Trigger win dialogue
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }

                        break;

                    case StepType.GiveItem:
                        if (inventory.Contains(currentStep.item))
                        {
                            giveItemEvent.RaiseEvent(inventory.GetKeyItem(currentStep.item));
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }

                        break;

                    case StepType.Dialogue:
                        //dialogue has already been played
                        if (currentStep.completeDialogue != null)
                        {
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            EndStep(currentStep);
                        }

                        break;
                    case StepType.KillEnemy:
                        if (enemyTracker.GetNumAngels(currentStep.angelBatch) >= currentStep.angelCount)
                        {
                            playCompletionDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            playIncompleteDialogueEvent.RaiseEvent();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void EndDialogue(int dialogueType)
        {

            //depending on the dialogue that ended, do something 
            switch ((DialogueType)dialogueType)
            {
                case DialogueType.CompletionDialogue:
                    if (_lastStepChecked.hasReward && _lastStepChecked.rewardItem != null)
                    {
                        rewardItemSoEvent.RaiseEvent(_lastStepChecked.rewardItem);
                    }
                    EndStep(_lastStepChecked);
                    break;
                case DialogueType.StartDialogue:
                    //TODO não sei se isto vai dar ou se é necessário até
                    CheckStepValidity(_lastStepChecked);
                    break;
            }
        }

        private void EndStep(StepSO step)
        {
            QuestSO quest = GetStepQuest(step);

            quest.currentStepIndex++;
            quest.currentStep.isDone = true;

            if (quest.currentStepIndex >= quest.steps.Count)
            {
                EndQuest(quest);
            }
            else
            {
                quest.currentStep = quest.steps[quest.currentStepIndex];
                _lastStepChecked = quest.currentStep;
            }
        }


        private static void EndQuest(QuestSO quest)
        {
            quest.FinishQuest();
            //saveSystem.SaveDataToDisk();
        }

    }
}