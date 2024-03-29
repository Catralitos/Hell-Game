﻿using System.Collections.Generic;
using Events.ScriptableObjects;
using Inventory.InstancedItems;
using Inventory.ScriptableObjects;
using Management;
using Management.ScriptableObjects;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        public TimeTrackerSO timeTracker;
        public InventorySO inventory;
        
        public List<GameObject> hearts;
        public TextMeshProUGUI timeText;
        public Image timeImage;

        public InventorySlot equippedItem;
        public TextMeshProUGUI itemName;
        
        public List<KeyItemDisplayer> keyItems;
        
        private const int StartingHours = 24 + 24 + 18;
        private int _currentHours;
        
        [Header("Listening on")] public IntEventChannelSO playerHealthEvent;
        public TimeChannelSO hourPassedEvent;
        public VoidEventChannelSO updateInventoryEvent;

        private void Start()
        {
            _currentHours = StartingHours;
        }

        private void OnEnable()
        {
            playerHealthEvent.OnEventRaised += UpdateHealth;
            hourPassedEvent.OnEventRaised += UpdateTimeFillEvent;
            updateInventoryEvent.OnEventRaised += UpdateKeyItems;
        }

        private void OnDisable()
        {
            playerHealthEvent.OnEventRaised -= UpdateHealth;
            hourPassedEvent.OnEventRaised -= UpdateTimeFillEvent;
            updateInventoryEvent.OnEventRaised -= UpdateKeyItems;
        }

        private void UpdateHealth(int health)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].SetActive(i < health);
            }
        }

        private void Update()
        {
            int hours = timeTracker.time.hour;
            int minutes = timeTracker.time.minute;
            
            string niceTime = $"{hours:00}:{minutes:00}";
            
            timeText.text = "Day " + timeTracker.time.day + " - " + niceTime;
                
            equippedItem.item = inventory.equippedItem;
            equippedItem.itemImage.sprite = equippedItem.item?.itemSprite;
            itemName.text = equippedItem.item?.itemName;
            switch (equippedItem.item)
            {
                case Weapon w:
                    equippedItem.leftText.text = w?.damage.ToString();
                    equippedItem.rightText.text = w?.usesLeft.ToString();
                    break;
                case HealingItem h:
                    equippedItem.leftText.text = h?.hpRestoreValue.ToString();
                    equippedItem.rightText.text = "HP";
                    break;
            }
        }

        private void UpdateTimeFillEvent(TimeStep time)
        {
            _currentHours--;
            timeImage.fillAmount = 1.0f * _currentHours / StartingHours;
        }

        private void UpdateKeyItems()
        {
            foreach (KeyItemDisplayer k in keyItems)
            {
                bool isIn = inventory.Contains(k.keyItem);
                if (isIn)
                {
                    if (!k.obtained) k.obtained = true;
                }

                k.inInventory = isIn;
                k.UpdateImage();
            }
        }
    }
}