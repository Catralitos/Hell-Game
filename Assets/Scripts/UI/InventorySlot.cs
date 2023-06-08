using Inventory;
using Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        private Image _itemImage;

        [FormerlySerializedAs("item")] public ItemSO itemSo;
    
        private void Start()
        {
            _itemImage = GetComponent<Image>();
            _itemImage.sprite = itemSo.itemSprite;
        }

        private void Update()
        {
            _itemImage.sprite = itemSo.itemSprite;
        }
    }
}
