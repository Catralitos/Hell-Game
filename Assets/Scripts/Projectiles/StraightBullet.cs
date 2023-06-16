using UnityEngine;

namespace Projectiles
{
    /// <summary>
    /// A class to set the player's bullets' trajectory
    /// </summary>
    /// <seealso cref="Bullets.Bullet" />
    public class StraightBullet : Bullet
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            OnObjectSpawn();
        }

        /// <summary>
        /// Called when [object spawn].
        /// </summary>
        public override void OnObjectSpawn()
        {
            Body.AddForce(Quaternion.Euler(0,0,-90) * transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
    }
}