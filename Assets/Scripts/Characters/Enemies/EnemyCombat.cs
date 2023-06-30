using System;
using Gameplay;
using UnityEngine;

namespace Characters.Enemies
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

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Debug.Log(_animator);
        }

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
            if (cooldownLeft <= 0)
            {
                _animator.SetTrigger("Attack");
                Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
                    attackSpawnPoint.position, attackRange, hittables);

                foreach (Collider2D hittable in hitObjects)
                {
                    hittable.gameObject.GetComponent<Hittable>().DoDamage(simpleDamage);
                }
            }

            cooldownLeft = attackCooldown;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackSpawnPoint.position, attackRange);
        }
    }
}