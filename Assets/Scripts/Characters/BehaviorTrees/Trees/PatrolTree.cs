using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.BehaviorTrees.EnemyTasks;

namespace Characters.BehaviorTrees.Trees
{
    public class PatrolTree : Selector
    {
        public PatrolTree(Enemy character, GameObject target, List<GameObject> patrolPoints)
        {
            this.children = new List<Task>();

            for (int i = 0; i < patrolPoints.Count; i++)
            {
                this.children.Add(
                        new SearchForTarget(
                            character,
                            target,
                            character.info.awakeDistance,
                            patrolPoints[i],
                            character.combat.attackRange
                         )
                 );
            }
        }
    }
}
