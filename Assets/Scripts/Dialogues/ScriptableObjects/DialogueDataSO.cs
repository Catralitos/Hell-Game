using System;
using System.Collections.Generic;
using System.Linq;
using Events.ScriptableObjects;
using Extensions;
using Management;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;

namespace Dialogues.ScriptableObjects
{
    public enum DialogueType
    {
        StartDialogue,
        CompletionDialogue,
        IncompletionDialogue,
        DefaultDialogue
    }

    public enum ChoiceActionType
    {
        DoNothing,
        ContinueWithStep,
        WinningChoice,
        LosingChoice,
        IncompleteStep
    }


    /// <summary>
    /// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
    /// In future versions it might contain support for branching conversations.
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogues/DialogueData")]
    public class DialogueDataSO : ScriptableObject
    {
        public List<Pair<int, List<Line>>> dialoguePerTime;
        public DialogueType dialogueType;
        public VoidEventChannelSO endOfDialogueEvent;

        public void FinishDialogue()
        {
            if (endOfDialogueEvent != null)
                endOfDialogueEvent.RaiseEvent();
        }

        public List<Line> GetLinesByHour(TimeStep time)
        {
            List<Line> toReturn = null;
            foreach (var pair in dialoguePerTime.Where(pair => time.hour >= pair.FirstMember))
            {
                toReturn = pair.SecondMember;
            }

            return toReturn;
        }
    }

    [Serializable]
    public class Choice
    {

        public LocalizedString response;
        public DialogueDataSO nextDialogue;
        public ChoiceActionType actionType;

        public void SetNextDialogue(DialogueDataSO dialogue)
        {
            nextDialogue = dialogue;
        }

        public Choice(Choice choice)
        {
            response = choice.response;
            nextDialogue = choice.nextDialogue;
            actionType = choice.actionType;
        }

        public Choice(LocalizedString response)
        {
            this.response = response;
        }

        public void SetChoiceAction(Comment comment)
        {
            actionType = (ChoiceActionType)Enum.Parse(typeof(ChoiceActionType), comment.CommentText);
            ;
        }
    }

    [Serializable]
    public class Line
    {
        public ActorID actorID;
        public List<LocalizedString> textList;
        public List<Choice> choices;

        public Line()
        {
            textList = null;
        }

        public void SetActor(Comment comment)
        {
            actorID = (ActorID)Enum.Parse(typeof(ActorID), comment.CommentText);
        }
    }
}
