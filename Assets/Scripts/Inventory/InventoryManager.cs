using Events.ScriptableObjects;
using Inventory.InstancedItems;
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
        public ItemEventChannelSO useItemSoEvent;
        //when a step/quest rewards you
        public ItemSOEventChannelSO rewardItemSoEvent;
        //when you give an NPC an object
        public ItemEventChannelSO giveItemSoEvent;
        //When you pick up an object
        public ItemSOEventChannelSO addItemSoEvent;

        [Header("Broadcasting on")] public VoidEventChannelSO updateInventoryEvent;
        
        private void OnEnable()
        {
            useItemSoEvent.OnEventRaised += UseItemEventRaised;
            addItemSoEvent.OnEventRaised += AddItem;
            rewardItemSoEvent.OnEventRaised += AddItem;
            giveItemSoEvent.OnEventRaised += RemoveItem;
        }

        private void OnDisable()
        {
            useItemSoEvent.OnEventRaised -= UseItemEventRaised;
            addItemSoEvent.OnEventRaised -= AddItem;
            rewardItemSoEvent.OnEventRaised -= AddItem;
            giveItemSoEvent.OnEventRaised -= RemoveItem;
        }
        
        private void AddItem(ItemSO item)
        {
            if (currentInventory.Add(item))
            {
                //Debug.Log("Added " + item.name + " to inventory");
                //_saveSystem.SaveDataToDisk();
                updateInventoryEvent.RaiseEvent();
            }
        }

        private void RemoveItem(Item item)
        {
            if (currentInventory.Remove(item))
            {
                //_saveSystem.SaveDataToDisk();
                updateInventoryEvent.RaiseEvent();
            }
        }
        
        private void UseItemEventRaised(Item item)
        {
            if (item is Weapon { usesLeft: > 0 })
            {
                updateInventoryEvent.RaiseEvent();
                return;
            }
            RemoveItem(item);
        }
    }
}