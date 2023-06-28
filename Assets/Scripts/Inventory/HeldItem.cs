using Inventory.InstancedItems;
using UnityEngine;

namespace Inventory
{
    public class HeldItem : MonoBehaviour
    {
        private SpriteRenderer _itemImage;

        public Item item;
        
        private void Start()
        {
            _itemImage = GetComponent<SpriteRenderer>();
            if (item != null) _itemImage.sprite = item.itemSprite;
        }

        private void Update()
        {
            _itemImage.sprite = item?.itemSprite;
        }
    }
}
