using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool, GameObject> { }

    /// <summary>
    /// A generic class for a "zone", that is a trigger collider that can detect if an object of a certain type (layer) entered or exited it.
    /// Implements <code>OnTriggerEnter</code> and <code>OnTriggerExit</code> so it needs to be on the same object that holds the Collider.
    /// </summary>
    public class ZoneTriggerController : MonoBehaviour
    {
        public BoolEvent enterZone;
        public LayerMask layers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (layers.HasLayer(other.gameObject.layer))
            {
                enterZone.Invoke(true, other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (layers.HasLayer(other.gameObject.layer))
            {
                enterZone.Invoke(false, other.gameObject);
            }
        }
    }
}