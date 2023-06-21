using System;
using UnityEngine;

namespace Management
{
    [Serializable]
    public class TimeStep
    {
        [Range(1,3)] public int day;
        public int hour;

        public TimeStep(int day, int hour)
        {
            this.day = day;
            this.hour = hour;
        }
    }
}