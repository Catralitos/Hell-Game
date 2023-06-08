using System;
using System.Collections.Generic;
using Events.ScriptableObjects;
using UnityEditor.Localization;
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
        DefaultDialogue,
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
        public List<Line> lines;
        public DialogueType dialogueType;
        public VoidEventChannelSO endOfDialogueEvent;

        public void FinishDialogue()
        {
            if (endOfDialogueEvent != null)
                endOfDialogueEvent.RaiseEvent();
        }


#if UNITY_EDITOR
        private void OnEnable()
        {
            SetDialogueLines(name);
        }

        public DialogueDataSO(string dialogueName)
        {
            SetDialogueLines(dialogueName);
        }

        private void SetDialogueLines(string dialogueName)
        {
            lines ??= new List<Line>();

            lines.Clear();
            int dialogueIndex = 0;
            Line dialogueLine = new Line();

            do
            {
                dialogueIndex++;
                dialogueLine = new Line("D" + dialogueIndex + "-" + dialogueName);
                if (dialogueLine.textList != null)
                    lines.Add(dialogueLine);

            } while (dialogueLine.textList != null);

#endif
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
            actionType = (ChoiceActionType)Enum.Parse(typeof(ChoiceActionType), comment.CommentText);;
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

#if UNITY_EDITOR
        public Line(string name)
        {
            StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");
            textList = null;
            if (collection != null)
            {

                int lineIndex = 0;
                LocalizedString dialogueLine = null;
                do
                {
                    lineIndex++;
                    string key = "L" + lineIndex + "-" + name;
                    if (collection.SharedData.Contains(key))
                    {

                        SetActor(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());
                        dialogueLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
                        textList ??= new List<LocalizedString>();
                        textList.Add(dialogueLine);
                    }
                    else
                    {
                        dialogueLine = null;
                    }

                } while (dialogueLine != null);

                int choiceIndex = 0;
                Choice choice = null;
                do
                {
                    choiceIndex++;
                    string key = "C" + choiceIndex + "-" + name;

                    if (collection.SharedData.Contains(key))
                    {
                        LocalizedString choiceLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
                        choice = new Choice(choiceLine);
                        choice.SetChoiceAction(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());

                        choices ??= new List<Choice>();
                        choices.Add(choice);
                    }
                    else
                    {
                        choice = null;
                    }
                } while (choice != null);
            }
            else
            {
                textList = null;
            }
        }
#endif
    }
}