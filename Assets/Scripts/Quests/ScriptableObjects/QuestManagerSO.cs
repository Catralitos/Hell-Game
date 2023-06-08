using System;
using System.Collections.Generic;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Quests.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Quests/Quest Manager")]
	public class QuestManagerSO : ScriptableObject
	{
		[Header("Data")] 
		public List<QuestlineSO> questlines;
		public InventorySO inventory;
		public ItemSO winningItem;
		public ItemSO losingItem;

		[Header("Listening to channels")]
		public VoidEventChannelSO continueWithStepEvent;
		public IntEventChannelSO endDialogueEvent;
		public VoidEventChannelSO makeWinningChoiceEvent;
		public VoidEventChannelSO makeLosingChoiceEvent;

		[Header("Broadcasting on channels")]
		public VoidEventChannelSO playCompletionDialogueEvent;
		public VoidEventChannelSO playIncompleteDialogueEvent;
		public ItemEventChannelSO giveItemEvent;
		public ItemEventChannelSO rewardItemEvent;
		//public SaveSystem saveSystem;

		private QuestSO _currentQuest = null;
		private QuestlineSO _currentQuestline;
		private StepSO _currentStep;
		private int _currentQuestlineIndex = 0;
		private int _currentQuestIndex = 0;
		private int _currentStepIndex = 0;

		public void OnDisable()
		{
			continueWithStepEvent.OnEventRaised -= CheckStepValidity;
			endDialogueEvent.OnEventRaised -= EndDialogue;
			makeWinningChoiceEvent.OnEventRaised -= MakeWinningChoice;
			makeLosingChoiceEvent.OnEventRaised -= MakeLosingChoice;
		}

		public void StartGame()
		{
			//Add code for saved information
			continueWithStepEvent.OnEventRaised += CheckStepValidity;
			endDialogueEvent.OnEventRaised += EndDialogue;
			makeWinningChoiceEvent.OnEventRaised += MakeWinningChoice;
			makeLosingChoiceEvent.OnEventRaised += MakeLosingChoice;
			StartQuestline();
		}

		private void StartQuestline()
		{
			if (questlines != null)
			{
				if (questlines.Exists(o => !o.isDone))
				{
					_currentQuestlineIndex = questlines.FindIndex(o => !o.isDone);

					if (_currentQuestlineIndex >= 0)
						_currentQuestline = questlines.Find(o => !o.isDone);
				}
			}
		}

		private bool HasStep(ActorSO actorToCheckWith)
		{
			if (_currentStep != null)
			{
				if (_currentStep.actor == actorToCheckWith)
				{
					return true;
				}
			}
			return false;
		}

		private bool CheckQuestlineForQuestWithActor(ActorSO actorToCheckWith)
		{
			if (_currentQuest == null)//check if there's a current quest 
			{
				if (_currentQuestline != null)
				{

					return _currentQuestline.quests.Exists(o => !o.isDone && o.steps != null && o.steps[0].actor == actorToCheckWith);

				}

			}
			return false;
		}

		public DialogueDataSO InteractWithCharacter(ActorSO actor, bool isCheckValidity, bool isValid)
		{
			if (_currentQuest == null)
			{
				if (CheckQuestlineForQuestWithActor(actor))
				{
					StartQuest(actor);
				}
			}

			if (HasStep(actor))
			{
				if (isCheckValidity)
				{
					return isValid ? _currentStep.completeDialogue : _currentStep.incompleteDialogue;
				}
				return _currentStep.dialogueBeforeStep;
			}
			return null;
		}

		//When Interacting with a character, we ask the quest manager if there's a quest that starts with a step with a certain character
		private void StartQuest(ActorSO actorToCheckWith)
		{
			if (_currentQuest != null)//check if there's a current quest 
			{
				return;
			}

			if (_currentQuestline != null)
			{
				//find quest index
				_currentQuestIndex = _currentQuestline.quests.FindIndex(o => !o.isDone && o.steps != null && o.steps[0].actor == actorToCheckWith);

				if ((_currentQuestline.quests.Count > _currentQuestIndex) && (_currentQuestIndex >= 0))
				{
					_currentQuest = _currentQuestline.quests[_currentQuestIndex];
					//start Step
					_currentStepIndex = 0;
					_currentStepIndex = _currentQuest.steps.FindIndex(o => o.isDone == false);
					if (_currentStepIndex >= 0)
						StartStep();
				}
			}
		}

		private void MakeWinningChoice()
		{
			//check if has sweet recipe
			_currentStep.item = winningItem;
			CheckStepValidity();
		}

		private void MakeLosingChoice()
		{
			_currentStep.item = losingItem;
			CheckStepValidity();
		}

		private void StartStep()
		{
			if (_currentQuest.steps != null)
				if (_currentQuest.steps.Count > _currentStepIndex)
				{
					_currentStep = _currentQuest.steps[_currentStepIndex];
				}
		}

		private void CheckStepValidity()
		{

			if (_currentStep != null)
			{
				switch (_currentStep.type)
				{
					case StepType.CheckItem:
						if (inventory.Contains(_currentStep.item))
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
						if (inventory.Contains(_currentStep.item))
						{
							giveItemEvent.RaiseEvent(_currentStep.item);
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
						if (_currentStep.completeDialogue != null)
						{
							playCompletionDialogueEvent.RaiseEvent();
						}
						else
						{
							EndStep();
						}
						break;
					case StepType.KillEnemy:
						//TODO ver como fazer para inimigo morto
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
					if (_currentStep.hasReward && _currentStep.rewardItem != null)
					{
						rewardItemEvent.RaiseEvent(_currentStep.rewardItem);
					}

					EndStep();
					break;
				case DialogueType.StartDialogue:
					CheckStepValidity();
					break;
				default:
					break;
			}
		}

		private void EndStep()
		{
			_currentStep = null;
			if (_currentQuest != null)
				if (_currentQuest.steps.Count > _currentStepIndex)
				{
					_currentQuest.steps[_currentStepIndex].FinishStep();
					//saveSystem.SaveDataToDisk();
					if (_currentQuest.steps.Count > _currentStepIndex + 1)
					{
						_currentStepIndex++;
						StartStep();

					}
					else
					{

						EndQuest();
					}
				}
		}

		private void EndQuest()
		{

			if (_currentQuest != null)
			{
				_currentQuest.FinishQuest();
				//saveSystem.SaveDataToDisk();
			}
			_currentQuest = null;
			_currentQuestIndex = -1;
			if (_currentQuestline != null)
			{
				if (!_currentQuestline.quests.Exists(o => !o.isDone))
				{
					EndQuestline();

				}
			}
		}

		private void EndQuestline()
		{
			if (questlines != null)
			{
				if (_currentQuestline != null)
				{
					_currentQuestline.FinishQuestline();
					//saveSystem.SaveDataToDisk();

				}
				if (questlines.Exists(o => o.isDone))
				{
					StartQuestline();
				}
			}
		}
	}
}
