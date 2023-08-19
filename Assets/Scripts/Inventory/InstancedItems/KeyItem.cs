using UnityEngine;
using System;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class KeyItem : Item
    {
        public KeyItem(string itemName, Sprite itemSprite) : base(itemName, itemSprite)
        {
        }
    }
}