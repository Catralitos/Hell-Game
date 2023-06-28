using Events.ScriptableObjects;
using Extensions;
using Gameplay;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class handles player health and getting hit
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerHealth : Hittable
    {
        /// <summary>
        /// The layers of the damaging objects
        /// </summary>
        public LayerMask damagers;

        public int collisionDamage;

        [Header("Broadcasting on")] 
        public IntEventChannelSO playerHealthEvent;
        
        [Header("Listening on")] 
        public IntEventChannelSO restoreHealthEvent;
        
        
        public void OnEnable()
        {
            restoreHealthEvent.OnEventRaised += RestoreHealth;
        }

        private void OnDisable()
        {
            restoreHealthEvent.OnEventRaised -= RestoreHealth;
        }

        /// <summary>
        /// Called when [collision stay2 d].
        /// </summary>
        /// <param name="other">The other.</param>
        private void OnCollisionStay2D(Collision2D other)
        {
            //if the collider hits a damaging item, do damage
            if (!damagers.HasLayer(other.gameObject.layer)) return;
            DoDamage(collisionDamage);
        }

        public override void DoDamage(int damage)
        {
            base.DoDamage(damage);
            playerHealthEvent.RaiseEvent(hitsLeft);
        }
        
        private void RestoreHealth(int amount)
        {
            hitsLeft = Mathf.Clamp(hitsLeft + amount, 0, maxHits);
            playerHealthEvent.RaiseEvent(hitsLeft);
        }
    }
}