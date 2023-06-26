using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.BehaviorTrees.EnemyTasks;

namespace Characters.BehaviorTrees.Trees
{
    public class SignalAngelsTree : Sequence
    {
        public SignalAngelsTree(
            Angel character, 
            GameObject target,
            GameObject backupTarget)
        {
            // To create a new tree you need to create each branch which is done using the constructors of different tasks
            // Additionally it is possible to create more complex behaviour by combining different tasks and composite tasks...
            this.children = new List<Task>()
                {
                    new IsCharacterNearTarget(character, target, character.enemyStats.AwakeDistance),
                    new SignalAngels(character, backupTarget),
                    new Chase(character, target,
                        character.enemyStats.WeaponRange,
                        1.5f * character.enemyStats.AwakeDistance)
                };

        }
    }
}
