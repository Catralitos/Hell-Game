using Events.ScriptableObjects;
using Extensions;
using Gameplay;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// This class handles enemy health and getting hit
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class EnemyHealth : Hittable
    {
        public EnemyHealth(): base() {
            this.maxHits = 3;
            this.hitsLeft = 3;
            this.invincibilityFrames = 0; // Cannot be invincible by default
        }

        protected override void Die()
        {
            Destroy(transform.parent.gameObject);
            //Debug.Log("Parent destroyed");
            base.Die();
        }
    }
}