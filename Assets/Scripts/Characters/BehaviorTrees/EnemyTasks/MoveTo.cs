using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    public class MoveTo : Task
    {
        public NPC Character { get; set; }

        public GameObject Target { get; set; }

        public float range;

        public MoveTo(Enemy character, GameObject target, float _range)
        {
            this.Character = character;
            this.Target = target;
            range = _range;
        }

        public override Result Run()
        {
            if (Target == null)
                return Result.Failure;

            if (Vector3.Distance(Character.transform.position, this.Target.transform.position) <= range)
            {
                Debug.Log("Reached Destination: " + this.Target.transform.position.x + "|" + this.Target.transform.position.y + "|" + this.Target.transform.position.z);
                return Result.Success;
            }
            else
            {
                Character.StartPathfinding(Target.transform.position);
                return Result.Running;
            }

        }

    }

}