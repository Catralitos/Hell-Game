using System.Collections.Generic;
using Gameplay.ScriptableObjects;
using Inventory.ScriptableObjects;
using Management.ScriptableObjects;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Management
{
    public class GameManager : MonoBehaviour
    {
        public GameStateSO gameState;
        public QuestManagerSO questManager;
        public InventorySO inventory;
        public EnemyTrackerSO enemyTracker;

        private void Awake()
        {
            gameState.Init();
            inventory.Init();
            enemyTracker.Init();
            questManager.Init();
        }
    }
}