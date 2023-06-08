using Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory
{
    public class HeldItem : MonoBehaviour
    {
        private SpriteRenderer _itemImage;

        [FormerlySerializedAs("item")] public ItemSO itemSo;
        
        private void Start()
        {
            _itemImage = GetComponent<SpriteRenderer>();
            _itemImage.sprite = itemSo.itemSprite;
        }

        private void Update()
        {
            _itemImage.sprite = itemSo != null ? itemSo.itemSprite : null;
        }
    }
}
