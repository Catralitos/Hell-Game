using System;
using UnityEngine;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class Item
    {
        public string itemName;
        public Sprite itemSprite;

        public Item(string itemName, Sprite itemSprite)
        {
            this.itemName = itemName;
            this.itemSprite = itemSprite;
        }
    }
}