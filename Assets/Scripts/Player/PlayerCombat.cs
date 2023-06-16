using System;
using Events.ScriptableObjects;
using Gameplay;
using Inventory;
using Inventory.ScriptableObjects;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {

        public HeldItem heldItem;
        
        [Header("Combat parameters")] 
        public float attackCooldown;
        private float _cooldownLeft;
        public LayerMask hittables;

        [Header("Melee Combat")] 
        public float attackRange;
        public Transform bulletSpawnPoint;

        [Header("Ranged Combat")] public GameObject bulletPrefab;
        
        [Header("Listening on")] public VoidEventChannelSO attackEvent;

        public void OnEnable()
        {
            attackEvent.OnEventRaised += Attack;
        }

        public void OnDisable()
        {
            attackEvent.OnEventRaised -= Attack;
        }

        private void Update()
        {
            _cooldownLeft -= Time.deltaTime;
        }

        private void Attack()
        {
            if (_cooldownLeft >= 0) return;
            //I can do this cast, because this event is only called when a weapon is equipped
            WeaponSO weapon = heldItem.itemSo as WeaponSO;

            if (weapon != null)
                switch (weapon.weaponType)
                {
                    case WeaponType.MELEE:
                        MeleeAttack(weapon);
                        break;
                    case WeaponType.RANGED:
                        RangedAttack(weapon);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        private void MeleeAttack(WeaponSO weapon)
        {
            Debug.Log("Melee Attack");
            GameObject o = heldItem.gameObject;
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
                o.transform.position * 1.5f, attackRange, hittables);

            foreach (Collider2D hittable in hitObjects)
            {
                hittable.gameObject.GetComponent<Hittable>().DoDamage(weapon.damage);
            }

            _cooldownLeft = attackCooldown;
        }

        private void RangedAttack(WeaponSO weapon)
        {
            Debug.Log("Ranged Attack");
            GameObject spawnedBullet =
                Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            spawnedBullet.GetComponent<StraightBullet>().bulletDamage = weapon.damage;
            _cooldownLeft = attackCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            GameObject o = heldItem.gameObject;
            Gizmos.DrawWireSphere(o.transform.position * 1.5f, attackRange);
        }
    }
}