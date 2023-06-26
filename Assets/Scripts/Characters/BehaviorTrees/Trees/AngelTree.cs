using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.BehaviorTrees.EnemyTasks;

namespace Characters.BehaviorTrees.Trees
{
    public class AngelTree : Selector
    {
        public AngelTree(
            Angel character, 
            GameObject target, 
            List<GameObject> patrolPositions,
            GameObject backupTarget)
        {
            this.children = new List<Task>()
            {
                new FoundPlayerTree(character, target, backupTarget),
                new PatrolTree(character, target, patrolPositions)
            };
        }
    }
}
