using Inventory.InstancedItems;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<Item> OnEventRaised;
	
        public void RaiseEvent(Item item)
        {
            OnEventRaised?.Invoke(item);
        }
    }
}