using System;
using UnityEngine;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class HealingItem : Item
    {
        public int hpRestoreValue;
        
        public HealingItem(string itemName, Sprite itemSprite, bool isKeyItem, int hpRestoreValue) : base(itemName, itemSprite, isKeyItem)
        {
            this.hpRestoreValue = hpRestoreValue;
        }
    }
}