using Audio;
using Extensions;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class handles player health and getting hit
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// The layers of the damaging objects
        /// </summary>
        public LayerMask damagers;

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
        /// How many hits the player can take before dying
        /// </summary>
        public int playerHits = 5;
        /// <summary>
        /// The hits left
        /// </summary>
        public int hitsLeft = 5;
        /// <summary>
        /// The number of invincibility frames
        /// </summary>
        public int invincibilityFrames;
        /// <summary>
        /// If the player is currently invincible
        /// </summary>
        private bool _invincible;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            hitsLeft = playerHits;
            _renderer = GetComponent<SpriteRenderer>();
            _defaultMaterial = _renderer.material;
        }

        /// <summary>
        /// Called when [collision stay2 d].
        /// </summary>
        /// <param name="other">The other.</param>
        private void OnCollisionStay2D(Collision2D other)
        {
            //if the collider hits a damaging item, do damage
            if (!damagers.HasLayer(other.gameObject.layer)) return;
            DoDamage();
        }

        /// <summary>
        /// Deals the damage.
        /// </summary>
        public void DoDamage()
        {
            if (_invincible) return;
            //Else deal damage
            if (hitsLeft > 1)
            {
                hitsLeft--;
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
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}