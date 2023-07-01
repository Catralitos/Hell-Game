using Events.ScriptableObjects;
using UnityEngine;

namespace Management
{
    public class ItemSpawnManager : MonoBehaviour
    {
        public int hoursBetweenRespawns;
        
        [Header("Listening On")]
        public TimeChannelSO hourPassedEvent;
        
        [Header("Broadcasting On")] 
        public VoidEventChannelSO spawnItemsEvent;
        
        private void Start()
        {
            spawnItemsEvent.RaiseEvent();
        }
        
        private void OnEnable()
        {
            hourPassedEvent.OnEventRaised += TimeIncreased;
        }

        private void OnDisable()
        {
            hourPassedEvent.OnEventRaised -= TimeIncreased;        
        }
        
        private void TimeIncreased(TimeStep time)
        {
            if (time.hour % hoursBetweenRespawns == 0)
            {
                spawnItemsEvent.RaiseEvent();
            }
        }
    }
}