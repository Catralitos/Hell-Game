using System.Collections.Generic;
using Inventory.ScriptableObjects;
using Management.ScriptableObjects;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Management
{
    public class GameManager : MonoBehaviour
    {
        public QuestManagerSO questManager;
        public InventorySO inventory;
        public EnemyTrackerSO enemyTracker;

        private void Start()
        {
            inventory.Init();
            enemyTracker.Init();
            questManager.Init();
        }
    }
}