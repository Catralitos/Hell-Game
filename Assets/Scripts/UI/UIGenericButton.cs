using Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace UI
{
    public class UIGenericButton : MonoBehaviour
    {
        public LocalizeStringEvent buttonText;
        public MultiInputButton button;

        public UnityAction Clicked;

        private bool _isDefaultSelection = false;

        private void OnDisable()
        {
            button.IsSelected = false;
            _isDefaultSelection = false;
        }

        public void SetButton(bool isSelect)
        {
            _isDefaultSelection = isSelect;
            if (isSelect)
                button.UpdateSelected();
        }

        public void SetButton(LocalizedString localizedString, bool isSelected)
        {
            buttonText.StringReference = localizedString;

            if (isSelected)
                SelectButton();
        }

        public void SetButton(string tableEntryReference, bool isSelected)
        {
            buttonText.StringReference.TableEntryReference = tableEntryReference;

            if (isSelected)
                SelectButton();
        }

        private void SelectButton()
        {
            button.Select();
        }

        public void Click()
        {
            Clicked.Invoke();
        }
    }
}