using System.Collections.Generic;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Events.ScriptableObjects.UI;
using Gameplay.ScriptableObjects;
using Inputs;
using Management;
using Management.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
	    
	public TimeTrackerSO timeTracker;
	public List<ActorSO> actorsList;
	public InputReader inputReader;
	public GameStateSO gameState;

	[Header("Listening on")]
	public DialogueDataChannelSO startDialogue;
	public DialogueChoiceChannelSO makeDialogueChoiceEvent;

	[Header("Broadcasting on")]
	public DialogueLineChannelSO openUIDialogueEvent;
	public DialogueChoicesChannelSO showChoicesUIEvent;
	public IntEventChannelSO endDialogueWithTypeEvent;
	public DialogueChoiceChannelSO continueWithStep;
	public VoidEventChannelSO playIncompleteDialogue;
	public DialogueActorChannelSO makeWinningChoice;
	public DialogueActorChannelSO makeLosingChoice;

	private int _counterDialogue;
	private int _counterLine;
	private DialogueDataSO _currentDialogue = default;

	private ActorSO _currentActor;
	
	private void Start()
	{
		startDialogue.OnEventRaised += DisplayDialogueData;
	}

	/// <summary>
	/// Displays DialogueData in the UI, one by one.
	/// </summary>
	public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
	{
		gameState.UpdateGameState(GameState.Dialogue);
		
		_counterDialogue = 0;
		_counterLine = 0;
		inputReader.EnableDialogueInput();
		inputReader.AdvanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;

		List<Line> lines = _currentDialogue.GetLinesByHour(timeTracker.time);
		if (lines != null)
		{
			_currentActor = actorsList.Find(o => o.actorId == lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
			DisplayDialogueLine(lines[_counterDialogue].textList[_counterLine], _currentActor);
			
		}
		else
		{
			Debug.LogError("Check Dialogue");
		}
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor)
	{
		openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
	}

	private void OnAdvance()
	{
		List<Line> lines = _currentDialogue.GetLinesByHour(timeTracker.time);

		_counterLine++;
		if (!(_counterLine >= lines[_counterDialogue].textList.Count))
		{
			_currentActor = actorsList.Find(o => o.actorId == lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
			DisplayDialogueLine(lines[_counterDialogue].textList[_counterLine], _currentActor);
		}
		else if (lines[_counterDialogue].choices != null
				&& lines[_counterDialogue].choices.Count > 0)
		{
			if (lines[_counterDialogue].choices.Count > 0)
			{
				DisplayChoices(lines[_counterDialogue].choices);
			}
		}
		else
		{
			_counterDialogue++;
			if (!(_counterDialogue >= lines.Count))
			{
				_counterLine = 0;

				_currentActor = actorsList.Find(o => o.actorId == lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
				DisplayDialogueLine(lines[_counterDialogue].textList[_counterLine], _currentActor);
			}
			else
			{
				DialogueEndedAndCloseDialogueUI();
			}
		}
	}

	private void DisplayChoices(List<Choice> choices)
	{
		inputReader.AdvanceDialogueEvent -= OnAdvance;

		makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
		showChoicesUIEvent.RaiseEvent(choices);
	}

	private void MakeDialogueChoice(Choice choice)
	{

		makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;

		switch (choice.actionType)
		{
			case ChoiceActionType.ContinueWithStep:
				if (continueWithStep != null)
					continueWithStep.RaiseEvent(choice);
				if (choice.nextDialogue != null)
					DisplayDialogueData(choice.nextDialogue);
				break;

			case ChoiceActionType.WinningChoice:
				if (makeWinningChoice != null)
					makeWinningChoice.RaiseEvent(_currentActor);
				break;

			case ChoiceActionType.LosingChoice:
				if (makeLosingChoice != null)
					makeLosingChoice.RaiseEvent(_currentActor);
				break;

			case ChoiceActionType.DoNothing:
				if (choice.nextDialogue != null)
					DisplayDialogueData(choice.nextDialogue);
				else
					DialogueEndedAndCloseDialogueUI();
				break;

			case ChoiceActionType.IncompleteStep:
				if (playIncompleteDialogue != null)
					playIncompleteDialogue.RaiseEvent();
				if (choice.nextDialogue != null)
					DisplayDialogueData(choice.nextDialogue);
				break;
		}
	}
	
	private void DialogueEndedAndCloseDialogueUI()
	{
		//raise the special event for end of dialogue if any 
		_currentDialogue.FinishDialogue();

		//raise end dialogue event 
		if (endDialogueWithTypeEvent != null)
			endDialogueWithTypeEvent.RaiseEvent((int)_currentDialogue.dialogueType);

		inputReader.AdvanceDialogueEvent -= OnAdvance;
		gameState.ResetToPreviousGameState();
		
		if (gameState.currentGameState == GameState.Gameplay)
			inputReader.EnableGameplayInput();
	}
}
}