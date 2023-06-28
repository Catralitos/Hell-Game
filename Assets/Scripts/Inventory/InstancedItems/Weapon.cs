using System;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Inventory.InstancedItems
{
    [Serializable]
    public class Weapon : Item
    {
        public int damage;
        public int usesLeft;

        public WeaponType weaponType;

        public Weapon(string itemName, Sprite itemSprite, bool isKeyItem, int damage, int usesLeft, WeaponType weaponType) : base(itemName, itemSprite, isKeyItem)
        {
            this.damage = damage;
            this.usesLeft = usesLeft;
            this.weaponType = weaponType;
        }
    }
}