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

        /* -1: current timestep is before timestep it is being compared to
         * 1: current timestep is after timestep it is being compared to
         * 0: current timestep is same timestep it is being compared to
         */
        public int Compare(TimeStep time)
        {
            if (day == time.day)
            {
                /* hour < time.hour: -1
                 * hour > time.hour: 1
                 * hour = time.hour: 0
                 */
                return hour.CompareTo(time.hour);
            } 
            else if (day < time.day)
            {
                return -1;
            } 
            else // if (day > time.day)
            {
                return 1;
            }
        }
    }
}