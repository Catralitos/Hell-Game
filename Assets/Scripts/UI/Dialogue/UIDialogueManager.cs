using System.Collections.Generic;
using Dialogues.ScriptableObjects;
using Events.ScriptableObjects.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace UI.Dialogue
{
    public class UIDialogueManager : MonoBehaviour
    {
        public LocalizeStringEvent lineText;
        public LocalizeStringEvent actorNameText;
        public GameObject actorNamePanel;
        public GameObject mainProtagonistNamePanel;
        public UIDialogueChoicesManager choicesManager;

        [Header("Listening to")] [SerializeField]
        private DialogueChoicesChannelSO showChoicesEvent;

        private void OnEnable()
        {
            showChoicesEvent.OnEventRaised += ShowChoices;
        }

        private void OnDisable()
        {
            showChoicesEvent.OnEventRaised -= ShowChoices;
        }

        public void SetDialogue(LocalizedString dialogueLine, ActorSO actor, bool isMainProtagonist)
        {
            choicesManager.gameObject.SetActive(false);
            lineText.StringReference = dialogueLine;

            actorNamePanel.SetActive(!isMainProtagonist);
            mainProtagonistNamePanel.SetActive(isMainProtagonist);

            if (!isMainProtagonist)
            {
                actorNameText.StringReference = actor.actorName;
            }
            //Protagonist's LocalisedString is provided on the GameObject already
        }

        private void ShowChoices(List<Choice> choices)
        {
            choicesManager.FillChoices(choices);
            choicesManager.gameObject.SetActive(true);
            Cursor.visible = true;
        }

        private void HideChoices()
        {
            choicesManager.gameObject.SetActive(false);
            Cursor.visible = false;
        }
    }
}
