using UnityEngine;

namespace Management
{
    [CreateAssetMenu(menuName = "Time Tracker")]
    public class TimeTrackerSO : ScriptableObject
    {
        public TimeStep time;
    }
}