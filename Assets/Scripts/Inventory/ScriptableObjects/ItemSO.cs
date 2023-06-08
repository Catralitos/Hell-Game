using UnityEngine;

namespace Inventory.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
        public new string name;
    
        public Sprite itemSprite;
    }
}
