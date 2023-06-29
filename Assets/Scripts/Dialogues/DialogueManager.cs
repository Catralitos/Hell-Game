using System.Collections.Generic;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects;
using Events.ScriptableObjects.UI;
using Gameplay.ScriptableObjects;
using Inputs;
using UnityEngine;
using UnityEngine.Localization;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
{
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
	private bool _reachedEndOfDialogue { get => _counterDialogue >= _currentDialogue.lines.Count; }
	private bool _reachedEndOfLine { get => _counterLine >= _currentDialogue.lines[_counterDialogue].textList.Count; }
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
		Debug.Log("Entrou");
		gameState.UpdateGameState(GameState.Dialogue);

		_counterDialogue = 0;
		_counterLine = 0;
		inputReader.EnableDialogueInput();
		inputReader.AdvanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;

		if (_currentDialogue.lines != null)
		{
			_currentActor = actorsList.Find(o => o.actorId == _currentDialogue.lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
			Debug.Log(_currentActor);
			DisplayDialogueLine(_currentDialogue.lines[_counterDialogue].textList[_counterLine], _currentActor);
			
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
		_counterLine++;
		if (!_reachedEndOfLine)
		{
			_currentActor = actorsList.Find(o => o.actorId == _currentDialogue.lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
			DisplayDialogueLine(_currentDialogue.lines[_counterDialogue].textList[_counterLine], _currentActor);
		}
		else if (_currentDialogue.lines[_counterDialogue].choices != null
				&& _currentDialogue.lines[_counterDialogue].choices.Count > 0)
		{
			if (_currentDialogue.lines[_counterDialogue].choices.Count > 0)
			{
				DisplayChoices(_currentDialogue.lines[_counterDialogue].choices);
			}
		}
		else
		{
			_counterDialogue++;
			if (!_reachedEndOfDialogue)
			{
				_counterLine = 0;

				_currentActor = actorsList.Find(o => o.actorId == _currentDialogue.lines[_counterDialogue].actorID); // we don't add a controle, because we need a null reference exeption if the actor is not in the list
				DisplayDialogueLine(_currentDialogue.lines[_counterDialogue].textList[_counterLine], _currentActor);
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