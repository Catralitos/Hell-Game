using Dialogues.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects.UI
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Choice Channel")]
    public class DialogueChoiceChannelSO : ScriptableObject
    {
        public UnityAction<Choice> OnEventRaised;
        public void RaiseEvent(Choice choice)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(choice);
        }
    }
}