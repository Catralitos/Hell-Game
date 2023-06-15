using Inventory.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public Image itemImage;
        public ItemSO itemSo;
        public TextMeshProUGUI leftText;
        public TextMeshProUGUI rightText;
    }
}
