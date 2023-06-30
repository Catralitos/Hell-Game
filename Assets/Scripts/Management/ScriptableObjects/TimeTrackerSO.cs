using UnityEngine;

namespace Management.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Time Tracker")]
    public class TimeTrackerSO : ScriptableObject
    {
        public TimeStep time;
    }
}