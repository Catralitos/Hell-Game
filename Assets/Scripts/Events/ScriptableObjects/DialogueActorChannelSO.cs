using Dialogues.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as paremater
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Dialogue Actor Channel")]
    public class DialogueActorChannelSO : ScriptableObject
    {
        public UnityAction<ActorSO> OnEventRaised;
	
        public void RaiseEvent(ActorSO actor)
        {
            OnEventRaised?.Invoke(actor);
        }
    }
}