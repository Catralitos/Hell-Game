using Events;
using UnityEngine;

namespace Management
{
    public class TimeManager : MonoBehaviour
    {

        public int dayInMinutes = 30;
        public int numberOfHours = 24 + 24 + 18;

        private int _currentDay = 1;
        private int _currentHour = 6;
        
        private int _numberOfSeconds;
        private float _hourLength;

        [Header("Broadcasting on")]
        public TimeChannelSO hourPassedEvent;
        
        public void Start()
        {
            _numberOfSeconds = dayInMinutes * 60;
            _hourLength = 1.0f * _numberOfSeconds / numberOfHours;
            InvokeRepeating(nameof(PassHour), _hourLength, _hourLength);
        }

        private void PassHour()
        {
            Debug.Log("An hour has passed");
            hourPassedEvent.RaiseEvent(new TimeStep(_currentDay, _currentHour));
            _currentHour++;
            if (_currentHour > 24)
            {
                _currentDay++;
                _currentHour = 1;
            }
        }
    }
}