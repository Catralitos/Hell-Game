﻿using Events.ScriptableObjects;
using Management.ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Management
{
    public class TimeManager : MonoBehaviour
    {
        public TimeTrackerSO timeTracker;
        
        [Header("Day Duration")]
        public int dayInMinutes = 30;
        public int numberOfHours = 24 + 24 + 18;

        private int _currentDay = 1;
        private int _currentHour = 6;
        private int _currentMinutes = 0;
        
        public Volume ppv; // this is the post processing volume
        public bool activateLights; // checks if lights are on
        public GameObject[] lights; // all the lights we want on when its dark

        [Header("Broadcasting on")]
        public TimeChannelSO hourPassedEvent;
        
        
        public void Start()
        {
            int numberOfSeconds = dayInMinutes * 60;
            float hourLength = 1.0f * numberOfSeconds / numberOfHours;
            float minuteLength = hourLength / 60;
            ppv.weight = 1;
            InvokeRepeating(nameof(IncreaseMinute), minuteLength, minuteLength);
            InvokeRepeating(nameof(PassHour), hourLength, hourLength);
        }

        private void IncreaseMinute()
        {
            _currentMinutes++;
            timeTracker.time = new TimeStep(_currentDay, _currentHour, _currentMinutes);
            ControlPPV();
        }

        private void PassHour()
        {
            _currentHour++;
            _currentMinutes = 0;
            if (_currentHour > 23)
            {
                _currentDay++;
                _currentHour = 0;
                if (_currentDay > 3) SceneManager.LoadScene(2);
            }

            TimeStep ts = new TimeStep(_currentDay, _currentHour, _currentMinutes);
            timeTracker.time = ts;
            hourPassedEvent.RaiseEvent(ts);
        }
        
        private void ControlPPV() // used to adjust the post processing slider.
        {
            ppv.weight = 0;
            switch (_currentHour)
            {
                case >= 22 or < 6:
                    ppv.weight = 1;
                    return;
                // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
                case >= 21 and < 22:
                {
                    ppv.weight =  (float)_currentMinutes / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
                
                    if (activateLights == false) // if lights havent been turned on
                    {
                        if (_currentMinutes > 45) // wait until pretty dark
                        {
                            foreach (GameObject t in lights)
                            {
                                t.SetActive(true); // turn them all on
                            }
                            activateLights = true;
                        }
                    }

                    break;
                }
                // Dawn at 6:00 / 6am    -   until 7:00 / 7am
                case >= 6 and < 7:
                {
                    ppv.weight = 1 - (float)_currentMinutes / 60; // we minus 1 because we want it to go from 1 - 0
                    if (activateLights) // if lights are on
                    {
                        if (_currentMinutes > 45) // wait until pretty bright
                        {
                            foreach (GameObject t in lights)
                            {
                                t.SetActive(false); // shut them off
                            }

                            activateLights = false;
                        }
                    }

                    break;
                }
                case >= 7 and < 21:
                    ppv.weight = 0;
                    return;
            }
        }
    }
}