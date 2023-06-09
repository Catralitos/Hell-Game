using Dialogues.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as parameter
    /// </summary>
    
    [CreateAssetMenu(menuName = "Events/Dialogue Data Channel")]
    public class DialogueDataChannelSO : ScriptableObject
    {
        public UnityAction<DialogueDataSO> OnEventRaised;
	
        public void RaiseEvent(DialogueDataSO dialogue)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(dialogue);
        }
    }
}