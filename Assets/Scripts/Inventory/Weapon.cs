using UnityEngine;

namespace Inventory
{
    public enum WeaponType
    {
        RANGED, 
        MELEE
    }

    [CreateAssetMenu]
    public class Weapon : InventoryItem
    {
        public int damage;
        public int usesLeft;

        public WeaponType weaponType;
    }
}