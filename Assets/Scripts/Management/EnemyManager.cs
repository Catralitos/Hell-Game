using Events.ScriptableObjects;
using Management.ScriptableObjects;
using UnityEngine;

namespace Management
{
    public class EnemyManager : MonoBehaviour
    {
        public EnemyTrackerSO enemyTracker;
        public int hoursBetweenRespawns;
        
        [Header("Listening On")] public IntEventChannelSO angelKilledEvent;
        public TimeChannelSO hourPassedEvent;

        [Header("Broadcasting On")] public VoidEventChannelSO spawnAngelsEvent;

        public int numAngelBatches = 8;

        private void Start()
        {
            enemyTracker.angelsKilled = new int[numAngelBatches + 1];
            //spawnAngelsEvent.RaiseEvent();
        }

        private void OnEnable()
        {
            angelKilledEvent.OnEventRaised += AddAngel;
            hourPassedEvent.OnEventRaised += TimeIncreased;
        }

        private void OnDisable()
        {
            angelKilledEvent.OnEventRaised -= AddAngel;
            hourPassedEvent.OnEventRaised -= TimeIncreased;        
        }

        private void AddAngel(int batch)
        {
            enemyTracker.AddAngel(batch);
        }

        private void TimeIncreased(TimeStep time)
        {
            if (time.hour % hoursBetweenRespawns == 0)
            {
                spawnAngelsEvent.RaiseEvent();
            }
        }
    }
}