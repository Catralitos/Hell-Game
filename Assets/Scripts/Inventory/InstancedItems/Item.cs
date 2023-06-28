using System;
using UnityEngine;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class Item
    {
        public string itemName;
        public Sprite itemSprite;
        public bool isKeyItem;

        public Item(string itemName, Sprite itemSprite, bool isKeyItem)
        {
            this.itemName = itemName;
            this.itemSprite = itemSprite;
            this.isKeyItem = isKeyItem;
        }
    }
}