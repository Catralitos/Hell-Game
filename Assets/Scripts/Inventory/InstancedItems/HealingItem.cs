using System;
using UnityEngine;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class HealingItem : Item
    {
        public int hpRestoreValue;
        
        public HealingItem(string itemName, Sprite itemSprite, int hpRestoreValue) : base(itemName, itemSprite)
        {
            this.hpRestoreValue = hpRestoreValue;
        }
    }
}