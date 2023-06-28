using UnityEngine;

namespace Gameplay
{
    public class Hittable : MonoBehaviour
    {
        
        /// <summary>
        /// The sprite renderer
        /// </summary>
        private SpriteRenderer _renderer;
        /// <summary>
        /// The default material
        /// </summary>
        private Material _defaultMaterial;
        /// <summary>
        /// The hit material
        /// </summary>
        public Material hitMaterial;
        /// <summary>
        /// How many hits the object can take before dying
        /// </summary>
        public int maxHits = 5;
        /// <summary>
        /// The hits left
        /// </summary>
        public int hitsLeft = 5;
        /// <summary>
        /// The number of invincibility frames
        /// </summary>
        public int invincibilityFrames;
        /// <summary>
        /// If the object is currently invincible
        /// </summary>
        private bool _invincible;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            hitsLeft = maxHits;
            _renderer = GetComponent<SpriteRenderer>();
            _defaultMaterial = _renderer.material;
        }
        
        
        /// <summary>
        /// Deals the damage.
        /// </summary>
        public virtual void DoDamage(int damage)
        {
            if (_invincible) return;
            //Else deal damage
            hitsLeft = Mathf.Clamp(hitsLeft - damage, 0, maxHits);
            if (hitsLeft > 0)
            {
                _renderer.material = hitMaterial;
                _invincible = true;
                Invoke(nameof(RestoreVulnerability), invincibilityFrames * Time.deltaTime);
            }
            else
            {
                Die();
            }
        }

        /// <summary>
        /// Restores the vulnerability.
        /// </summary>
        private void RestoreVulnerability()
        {
            _invincible = false;
            _renderer.material = _defaultMaterial;
        }

        /// <summary>
        /// Kill the player
        /// </summary>
        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}