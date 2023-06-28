using UnityEngine;

namespace Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory Items/Item")]
    public class ItemSO : ScriptableObject
    {
        public new string name;
    
        public Sprite itemSprite;

        public bool isKeyItem;
    }
}
