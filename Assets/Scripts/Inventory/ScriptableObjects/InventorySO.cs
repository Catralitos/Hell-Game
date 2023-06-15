using System.Collections.Generic;
using UnityEngine;

namespace Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory")]
    public class InventorySO : ScriptableObject
    {
        public int maxInventoryItems;
        [Tooltip("The collection of items and their quantities.")]
        public List<ItemSO> items = new List<ItemSO>();
        public List<ItemSO> defaultItems = new List<ItemSO>();
        public List<ItemSO> keyItems = new List<ItemSO>();
        
        public void Init()
        {
            items ??= new List<ItemSO>();
            items.Clear();
            foreach (ItemSO item in defaultItems)
            {
                items.Add(item);
            }
        }

        public bool Add(ItemSO item)
        {
            if (!item.isKeyItem)
            {
                if (items.Count <= maxInventoryItems)
                {
                    return false;
                }

                items.Add(item);
                return true;
            }
            keyItems.Add(item);
            return true;
        }

        public bool Remove(ItemSO item)
        {
            if (!items.Contains(item) || !keyItems.Contains(item)) return false;
            if (!item.isKeyItem)
            {
                items.Remove(item);
                return true;
            }
            keyItems.Remove(item);
            return true;
        }
        
        public bool Contains(ItemSO item)
        {
            return items.Contains(item) || keyItems.Contains(item);
        }

    }
}