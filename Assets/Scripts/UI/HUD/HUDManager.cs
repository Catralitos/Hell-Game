using System.Collections.Generic;
using Events.ScriptableObjects;
using Inventory.ScriptableObjects;
using Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        public InventorySO inventory;
        
        public List<GameObject> hearts;
        public TextMeshProUGUI timeText;
        public Image timeImage;

        public List<KeyItemDisplayer> keyItems;
        
        private const int StartingHours = 24 + 24 + 18;
        private int _currentHours;
        
        [Header("Listening on")] public IntEventChannelSO playerHealthEvent;
        public TimeChannelSO minutePassedEvent;
        public TimeChannelSO hourPassedEvent;
        public VoidEventChannelSO updateInventoryEvent;

        private void Start()
        {
            _currentHours = StartingHours;
        }

        private void OnEnable()
        {
            playerHealthEvent.OnEventRaised += UpdateHealth;
            minutePassedEvent.OnEventRaised += UpdateTimeText;
            hourPassedEvent.OnEventRaised += UpdateTimeFillEvent;
            updateInventoryEvent.OnEventRaised += UpdateKeyItems;
        }

        private void OnDisable()
        {
            playerHealthEvent.OnEventRaised -= UpdateHealth;
            minutePassedEvent.OnEventRaised -= UpdateTimeText;
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

        private void UpdateTimeText(TimeStep time)
        {
            int hours = time.hour;
            int minutes = time.minute;
            
            string niceTime = $"{hours:00}:{minutes:00}";
            
            timeText.text = "Day " + time.day + " - " + niceTime;
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