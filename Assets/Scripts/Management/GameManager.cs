using System;
using System.Collections.Generic;
using Audio;
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
        public TimeTrackerSO timeTracker;
        
        private AudioManager _audioManager;
        private void Awake()
        {
            gameState.Init();
            inventory.Init();
            enemyTracker.Init();
            questManager.Init();
            timeTracker.Init();
        }

        private void Start()
        {
            _audioManager = GetComponent<AudioManager>();
            _audioManager.Play("GameMusic");
        }
    }
}