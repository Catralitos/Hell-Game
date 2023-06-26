using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    public class LightAttack : Task
    {
        protected Enemy character { get; set; }

        public GameObject target { get; set; }

        public LightAttack(Enemy character)
        {
            this.character = character;
        }
        public override Result Run()
        {
            character.AttackPlayer();
            return Result.Success;
        }
    }
}
