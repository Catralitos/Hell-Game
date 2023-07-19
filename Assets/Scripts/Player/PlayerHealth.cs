using Audio;
using Events.ScriptableObjects;
using Extensions;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private AudioManager _audioManager;

        public override void Start()
        {
            base.Start();
            _audioManager = GetComponent<AudioManager>();
        }
        
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
            if (_invincible) return;
            //Else deal damage
            hitsLeft = Mathf.Clamp(hitsLeft - damage, 0, maxHits);
            if (hitsLeft > 0)
            {
                if (hitMaterial != null) renderer.material = hitMaterial;
                _invincible = true;
                Physics2D.IgnoreLayerCollision(6, 8, true);
                Invoke(nameof(RestoreVulnerability), invincibilityFrames / 60.0f);
                Debug.Log(transform.name + " got hit");
            }
            else
            {
                Debug.Log(transform.name + " died");
                Die();
            }
            _audioManager.Play("Hit");
            playerHealthEvent.RaiseEvent(hitsLeft);
        }
        
        protected override void RestoreVulnerability()
        {
             
            _invincible = false;
            Physics2D.IgnoreLayerCollision(6, 8, false);
            renderer.material = defaultMaterial;
        }
        
        private void RestoreHealth(int amount)
        {
            hitsLeft = Mathf.Clamp(hitsLeft + amount, 0, maxHits);
            _audioManager.Play("Heal");
            playerHealthEvent.RaiseEvent(hitsLeft);
        }
        
        protected override void Die()
        {
            _audioManager.Play("Death");
            SceneManager.LoadScene(4);
           base.Die();
        }
    }
}