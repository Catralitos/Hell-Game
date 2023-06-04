using UnityEngine;

namespace Inventory
{
    public class HeldItem : MonoBehaviour
    {
        private SpriteRenderer _itemImage;

        public InventoryItem item;
    
        private void Start()
        {
            _itemImage = GetComponent<SpriteRenderer>();
            _itemImage.sprite = item.itemSprite;
        }

        private void Update()
        {
            _itemImage.sprite = item != null ? item.itemSprite : null;
        }
    }
}
