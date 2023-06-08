using UnityEngine;

namespace Inventory.ScriptableObjects
{
    public enum WeaponType
    {
        RANGED, 
        MELEE
    }

    [CreateAssetMenu(menuName = "Inventory Items/Weapon")]
    public class WeaponSO : ItemSO
    {
        public int damage;
        public int usesLeft;

        public WeaponType weaponType;
    }
}