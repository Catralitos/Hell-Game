using Inventory.InstancedItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public Image itemImage;
        public Item item;
        public TextMeshProUGUI leftText;
        public TextMeshProUGUI rightText;
    }
}
