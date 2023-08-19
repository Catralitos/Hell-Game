using System;
using Inventory.ScriptableObjects;
using UnityEngine;

namespace Inventory
{
    public class CollectableItem : MonoBehaviour
    {
        public ItemSO currentItem;
        private SpriteRenderer _spriteRenderer;

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = currentItem.itemSprite;
        }

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