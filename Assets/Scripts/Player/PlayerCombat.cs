using System;
using Events.ScriptableObjects;
using Gameplay;
using Inventory;
using Inventory.InstancedItems;
using Inventory.ScriptableObjects;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {

        public Transform itemHolder;
        public HeldItem heldItem;
        
        [Header("Combat parameters")] 
        public float attackCooldown;
        private float _cooldownLeft;
        public LayerMask hittables;

        [Header("Melee Combat")] 
        public float attackRange;
        public Transform attackSpawnPoint;

        [Header("Ranged Combat")] 
        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;

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

            if (heldItem.item is Weapon weapon)
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

        private void MeleeAttack(Weapon weapon)
        {
            //Debug.Log("Melee Attack");
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
                attackSpawnPoint.position, attackRange, hittables);

            foreach (Collider2D hittable in hitObjects)
            {
                hittable.gameObject.GetComponent<Hittable>().DoDamage(weapon.damage);
            }

            _cooldownLeft = attackCooldown;
        }

        private void RangedAttack(Weapon weapon)
        {
            Debug.Log("Ranged Attack");
            GameObject spawnedBullet =
                Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            spawnedBullet.GetComponent<StraightBullet>().bulletDamage = weapon.damage;
            _cooldownLeft = attackCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackSpawnPoint.position, attackRange);
        }
    }
}