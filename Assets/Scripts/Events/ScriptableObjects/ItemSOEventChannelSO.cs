using Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Item interaction events.
    /// Example: Pick up an item passed as paramater
    /// </summary>

    [CreateAssetMenu(menuName = "Events/UI/ItemSO Event Channel")]
    public class ItemSOEventChannelSO : ScriptableObject
    {
        public UnityAction<ItemSO> OnEventRaised;
	
        public void RaiseEvent(ItemSO item)
        {
            OnEventRaised?.Invoke(item);
        }
    }
}