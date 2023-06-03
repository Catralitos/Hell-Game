using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Singleton to easily access all the player's components
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerEntity : MonoBehaviour
    {

        /// <summary>
        /// The PlayerMovement
        /// </summary>
        [HideInInspector] public PlayerInput input;
        /// <summary>
        /// The PlayerMovement
        /// </summary>
        [HideInInspector] public PlayerMovement movement;
        /// <summary>
        /// The PlayerHealth
        /// </summary>
        [HideInInspector] public PlayerHealth health;
        /// <summary>
        /// The PlayerRanged
        /// </summary>
        [HideInInspector] public PlayerRanged ranged;
        /// <summary>
        /// The PlayerMelee
        /// </summary>
        [HideInInspector] public PlayerMelee melee;
        
        #region SingleTon

        /// <summary>
        /// Gets the sole instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PlayerEntity Instance { get; private set; }

        /// <summary>
        /// Awakes this instance (if none exist).
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                input = GetComponent<PlayerInput>();
                movement = GetComponent<PlayerMovement>();
                health = GetComponent<PlayerHealth>();
                ranged = GetComponent<PlayerRanged>();
                melee = GetComponent<PlayerMelee>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}