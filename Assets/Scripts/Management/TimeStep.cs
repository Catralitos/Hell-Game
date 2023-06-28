using System;
using UnityEngine;

namespace Management
{
    [Serializable]
    public class TimeStep
    {
        [Range(1,3)] public int day;
        public int hour;
        public int minute;

        public TimeStep(int day, int hour, int minute)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }
    }
}