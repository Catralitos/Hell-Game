using UnityEngine;

namespace Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory Items/Healing Item")]
    public class HealingItemSO : ItemSO
    {
        public int hpRestoreValue;
    }
}
