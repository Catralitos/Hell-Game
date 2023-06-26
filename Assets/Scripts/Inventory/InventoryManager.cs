using Events.ScriptableObjects;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
	    
        public InventorySO currentInventory;
        //[SerializeField] private SaveSystem _saveSystem;

        [Header("Listening on")]
        //When you use an item and need to see if it is removed
        public ItemEventChannelSO useItemEvent;
        //when a step/quest rewards you
        public ItemEventChannelSO rewardItemEvent;
        //when you give an NPC an object
        public ItemEventChannelSO giveItemEvent;
        //When you pick up an object
        public ItemEventChannelSO addItemEvent;

        [Header("Broadcasting on")] public VoidEventChannelSO updateInventoryEvent;
        
        private void OnEnable()
        {
            useItemEvent.OnEventRaised += UseItemEventRaised;
            addItemEvent.OnEventRaised += AddItem;
            rewardItemEvent.OnEventRaised += AddItem;
            giveItemEvent.OnEventRaised += RemoveItem;
        }

        private void OnDisable()
        {
            useItemEvent.OnEventRaised -= UseItemEventRaised;
            addItemEvent.OnEventRaised -= AddItem;
            rewardItemEvent.OnEventRaised -= AddItem;
            giveItemEvent.OnEventRaised -= RemoveItem;
        }
        
        private void AddItem(ItemSO item)
        {
            Debug.Log("Chamou AddItem");
            if (currentInventory.Add(item))
            {
                //_saveSystem.SaveDataToDisk();
                updateInventoryEvent.RaiseEvent();
            }
        }

        private void RemoveItem(ItemSO item)
        {
            if (currentInventory.Remove(item))
            {
                //_saveSystem.SaveDataToDisk();
                updateInventoryEvent.RaiseEvent();
            }
        }
        
        private void UseItemEventRaised(ItemSO item)
        {
            if (item is WeaponSO weapon)
            {
                //Raise event for combat
                weapon.usesLeft--;
                //_saveSystem.SaveDataToDisk();
                if (weapon.usesLeft > 0) return;
            }
            RemoveItem(item);
        }
    }
}