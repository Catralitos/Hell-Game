using System.Collections.Generic;
using Dialogues.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects.UI
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Choices Channel")]
    public class DialogueChoicesChannelSO : ScriptableObject
    {
        public UnityAction<List<Choice>> OnEventRaised;
	
        public void RaiseEvent(List<Choice> choices)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(choices);
        }
    }
}