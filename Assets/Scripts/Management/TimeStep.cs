using System;
using UnityEngine;

namespace Management
{
    [Serializable]
    public class TimeStep
    {
        [Range(1,3)] public int day;
        [Range(1,24)] public int hour;
        [HideInInspector] public int minute;

        public TimeStep(int day, int hour, int minute)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }

        public int Compare(TimeStep time)
        {
            if (day == time.day)
            {
                return hour.CompareTo(time.hour);
            } 
            if (day < time.day)
            {
                return -1;
            } 
            if (day > time.day)
            {
                return 1;
            }

            return -1;
        }
    }
}