using System;
using Events.ScriptableObjects;
using Gameplay;
using Inventory;
using Inventory.ScriptableObjects;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class EnemyCombat : MonoBehaviour
    {
        [Header("Combat parameters")] 
        public float attackCooldown;
        public float cooldownLeft;
        public int simpleDamage;
        public LayerMask hittables;
        public int AC;

        public Func<int> DmgRoll;

        [Header("Melee Combat")] 
        public float attackRange;
        public Transform attackSpawnPoint;

        private void Update()
        {
            cooldownLeft -= Time.deltaTime;
        }

        public void Attack()
        {
            MeleeAttack();
        }

        private void MeleeAttack()
        {
            Debug.Log("Enemy - used Melee Attack");
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
                attackSpawnPoint.position, attackRange, hittables);

            foreach (Collider2D hittable in hitObjects)
            {
                hittable.gameObject.GetComponent<Hittable>().DoDamage(simpleDamage);
            }

            cooldownLeft = attackCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackSpawnPoint.position, attackRange);
        }
    }
}