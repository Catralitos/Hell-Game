using Inventory.ScriptableObjects;
using UnityEngine;

namespace Inventory
{
    public class CollectableItem : MonoBehaviour
    {
        public ItemSO currentItem;
        
        public ItemSO GetItem()
        {
            return currentItem;
        }

        public void SetItem(ItemSO item)
        {
            currentItem = item;
        }

    }
}