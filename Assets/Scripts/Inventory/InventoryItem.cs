using UnityEngine;

namespace Inventory
{
    public abstract class InventoryItem : ScriptableObject
    {
        public new string name;
    
        public Sprite itemSprite;
    }
}
