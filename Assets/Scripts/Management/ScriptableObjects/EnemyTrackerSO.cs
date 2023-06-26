using UnityEngine;

namespace Management.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Enemy Tracker")]
    public class EnemyTrackerSO : ScriptableObject
    {
        public int[] angelsKilled;
        
        public void AddAngel(int batch)
        {
            angelsKilled[batch + 1]++;
        }

        public int GetNumAngels(int batch)
        {
            return angelsKilled[batch + 1];
        }
    }
}