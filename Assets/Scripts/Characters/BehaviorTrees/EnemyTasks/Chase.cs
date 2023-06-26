using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    public class Chase : Task
    {
        public NPC Character { get; set; }

        public GameObject Target { get; set; }

        public float rangeMin;

        public float rangeMax;

        public Chase(Enemy character, GameObject target, float min, float max)
        {
            this.Character = character;
            this.Target = target;
            rangeMin = min;
            rangeMax = max;
        }

        public override Result Run()
        {
            if (Target == null)
                return Result.Failure;

            if (Vector3.Distance(Character.transform.position, this.Target.transform.position) <= rangeMin)
            {
                Debug.Log("Reached Destination: " + this.Target.transform.position.x + "|" + this.Target.transform.position.y + "|" + this.Target.transform.position.z);
                return Result.Success;
            }

            else if (Vector3.Distance(Character.transform.position, this.Target.transform.position) >= rangeMax)
            {
                Debug.Log("Target out of range: " + this.Target.transform.position.x + "|" + this.Target.transform.position.y + "|" + this.Target.transform.position.z);
                Character.StartPathfinding(Character.transform.position);
                return Result.Failure;
            }

            else
            {
                Character.StartPathfinding(Target.transform.position);
                return Result.Running;
            }

        }
    }
}
