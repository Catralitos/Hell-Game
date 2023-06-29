using System.Collections.Generic;
using System.Linq;
using Inventory.InstancedItems;
using UnityEngine;

namespace Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory")]
    public class InventorySO : ScriptableObject
    {
        public int maxInventoryItems;
        [Tooltip("The collection of items and their quantities.")]
        public List<Item> items = new List<Item>();
        public List<KeyItem> keyItems = new List<KeyItem>();
        public List<ItemSO> defaultItems = new List<ItemSO>();
        
        public void Init()
        {
            items ??= new List<Item>();
            items.Clear();
            keyItems.Clear();
            foreach (ItemSO item in defaultItems)
            {
                switch (item)
                {
                    case HealingItemSO heal:
                        items.Add(new HealingItem(heal.name, heal.itemSprite, heal.hpRestoreValue));
                        break;
                    case WeaponSO weapon:
                        items.Add(new Weapon(weapon.name, weapon.itemSprite, weapon.damage, weapon.usesLeft, weapon.weaponType));
                        break;
                    case KeyItemSO keyItem:
                        keyItems.Add(new KeyItem(keyItem.name, keyItem.itemSprite));
                        break;
                }
            }
        }

        public bool Add(ItemSO item)
        {
            switch (item)
            {
                case HealingItemSO heal:
                    if (items.Count >= maxInventoryItems) return false;
                    items.Add(new HealingItem(heal.name, heal.itemSprite, heal.hpRestoreValue));
                    return true;
                case WeaponSO weapon:
                    if (items.Count >= maxInventoryItems) return false;
                    items.Add(new Weapon(weapon.name, weapon.itemSprite, weapon.damage, weapon.usesLeft, weapon.weaponType));
                    return true;
                case KeyItemSO keyItem:
                    keyItems.Add(new KeyItem(keyItem.name, keyItem.itemSprite));
                    return true;
            }

            return false;
        }

        public bool Remove(Item item)
        {
            if (!items.Contains(item) || !keyItems.Contains(item)) return false;
            if (item is KeyItem keyItem)
            {
                keyItems.Remove(keyItem);
                return true;
            }
            items.Remove(item);
            return true;

        }
        
        public bool Contains(Item item)
        {
            return items.Contains(item) || keyItems.Contains(item);
        }
        
        public bool Contains(ItemSO item)
        {
            List<Item> aux = items.Union(keyItems).ToList();
            return aux.Any(i => i.itemName == item.name);
        }

        public KeyItem GetKeyItem(ItemSO item)
        {
            return keyItems.FirstOrDefault(k => k.itemName == item.name);
        }
    }
}