using Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Item interaction events.
    /// Example: Pick up an item passed as paramater
    /// </summary>

    [CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<ItemSO> OnEventRaised;
	
        public void RaiseEvent(ItemSO item)
        {
            OnEventRaised?.Invoke(item);
        }
    }
}