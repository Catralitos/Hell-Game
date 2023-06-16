using System;
using Extensions;
using Gameplay;
using UnityEngine;

namespace Projectiles
{
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// The player layer
        /// </summary>
        public LayerMask hittables;
        /// <summary>
        /// The walls layer
        /// </summary>
        public LayerMask wallsLayer;

        public float bulletLifetime = 5f;
        private float _timePassed;
        
        /// <summary>
        /// The explosion prefab
        /// </summary>
        //public GameObject explosionPrefab;
        
        /// <summary>
        /// The bullet damage
        /// </summary>
        public int bulletDamage = 1;
        /// <summary>
        /// The bullet speed
        /// </summary>
        public float bulletSpeed = 20.0f;

        /// <summary>
        /// The rigidbody
        /// </summary>
        protected Rigidbody2D Body;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= bulletLifetime)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Called when [trigger enter2 d].
        /// </summary>
        /// <param name="col">The col.</param>
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (hittables.HasLayer(col.gameObject.layer))
            {
                col.gameObject.GetComponent<Hittable>().DoDamage(bulletDamage);
            }
            if (wallsLayer.HasLayer(col.gameObject.layer))
            {
                Destroy(gameObject);
            }
            //Instantiate(explosionPrefab, transform.position, quaternion.identity);
        }

        /// <summary>
        /// Called when [object spawn].
        /// </summary>
        public virtual void OnObjectSpawn()
        {
            //do nothing, each bullet will know what to do
        }

        /// <summary>
        /// Called when [object spawn].
        /// </summary>
        /// <param name="angle">The angle.</param>
        public virtual void OnObjectSpawn(float angle)
        {
            //do nothing, each bullet will know what to do
        }

        /// <summary>
        /// Called when [object spawn].
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="maxAngleStep">The maximum angle step.</param>
        public virtual void OnObjectSpawn(float angle, float maxAngleStep)
        {
            //do nothing, each bullet will know what to do
        }
    }
}