using Dialogues.ScriptableObjects;
using Events.ScriptableObjects.UI;
using Menu;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace UI.Dialogue
{
    public class UIDialogueChoiceFiller : MonoBehaviour
    {
        public LocalizeStringEvent choiceText;
        public MultiInputButton actionButton;

        [Header("Broadcasting on")]
        public DialogueChoiceChannelSO onChoiceMade;

        private Choice _currentChoice;

        public void FillChoice(Choice choiceToFill, bool isSelected)
        {
            _currentChoice = choiceToFill;
            choiceText.StringReference = choiceToFill.response;
            actionButton.interactable = true;

            if (isSelected)
            {
                actionButton.UpdateSelected();
            }
        }

        public void ButtonClicked()
        {
            onChoiceMade.RaiseEvent(_currentChoice);
        }
    }
}