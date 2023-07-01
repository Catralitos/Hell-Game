using Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class KeyItemDisplayer : MonoBehaviour
    {
        public Sprite hiddenSprite;
        public KeyItemSO keyItem;
        [HideInInspector] public bool inInventory;
        [HideInInspector] public bool obtained;
        
        private Image _image;
        
        private void Start()
        {
            _image = GetComponent<Image>();
            _image.sprite = hiddenSprite;
        }

        public void UpdateImage()
        {
            if (obtained)
            {
                _image.sprite = keyItem.itemSprite;
            }

            if (_image != null) _image.color = inInventory ? Color.white : Color.gray;
        }
    }
}