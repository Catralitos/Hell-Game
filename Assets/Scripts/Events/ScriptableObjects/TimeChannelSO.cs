using Management;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Time Event Channel")]
    public class TimeChannelSO : ScriptableObject
    {
        public UnityAction<TimeStep> OnEventRaised;
        
        public void RaiseEvent(TimeStep timeStep)
        {
            OnEventRaised?.Invoke(timeStep);
        }
    }
}