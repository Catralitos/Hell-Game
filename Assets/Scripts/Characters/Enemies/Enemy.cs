using Characters.CharacterPathfinding;
using UnityEngine;

namespace Characters.Enemies
{
    public class Enemy : PathfindingEntity
    {
        public struct EnemyInfo
        {
            public string Type;
            public float awakeDistance;
        }

        public EnemyInfo info;

        [HideInInspector] public EnemyCombat combat;
        [HideInInspector] public EnemyHealth health;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            this.combat = GetComponent<EnemyCombat>();
            this.health = GetComponent<EnemyHealth>();
            this.health.canBecomeInvincible = false;
            this.health._invincible = false;
            this.info = new EnemyInfo();
            this.AIDestinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
            this.ai.isStopped = true;
        }

        public virtual void Update()
        {
            if (this.ai.remainingDistance <= this.info.awakeDistance)
                this.ai.isStopped = false;

            if (this.ai.reachedDestination && this.ai.canMove)
            {
                AttackPlayer();
            }
        }


        public void AttackPlayer()
        {
            combat.Attack();
        }
    }
}
