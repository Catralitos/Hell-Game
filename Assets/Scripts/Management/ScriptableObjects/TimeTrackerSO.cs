using UnityEngine;

namespace Management.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Time Tracker")]
    public class TimeTrackerSO : ScriptableObject
    {
        public TimeStep time;

        public void Init()
        {
            time = new TimeStep(1, 6, 0);
        }
    }
}