using System;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        private Image _itemImage;

        public InventoryItem item;
    
        // Start is called before the first frame update
        private void Start()
        {
            _itemImage = GetComponent<Image>();
            _itemImage.sprite = item.itemSprite;
        }

        private void OnValidate()
        {
            Start();
        }
    }
}
