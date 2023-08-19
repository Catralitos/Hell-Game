using Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events to toggle the interaction UI.
    /// Example: Dispaly or hide the interaction UI via a bool and the interaction type from the enum via int
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Toggle Interaction UI Event Channel")]
    public class InteractionUIEventChannelSO : ScriptableObject
    {
        public UnityAction<bool, InteractionType> OnEventRaised;

        public void RaiseEvent(bool state, InteractionType interactionType)
        {
            OnEventRaised?.Invoke(state, interactionType);
        }
    }
}