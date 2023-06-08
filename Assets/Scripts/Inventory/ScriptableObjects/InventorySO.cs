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
        
        public void Init()
        {
            items ??= new List<ItemSO>();
            items.Clear();
            foreach (ItemSO item in defaultItems)
            {
                items.Add(item);
            }
        }

        public void Add(ItemSO item)
        {
            if (items.Count <= maxInventoryItems)
            {
                return;
            }
            items.Add(item);
        }

        public void Remove(ItemSO item)
        {
            items.Remove(item);
        }
        
        public bool Contains(ItemSO item)
        {
            return items.Contains(item);
        }

    }
}