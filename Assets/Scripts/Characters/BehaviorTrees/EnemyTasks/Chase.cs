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

            Debug.Log("CHASE(" + Character.gameObject.name + "). Target = " + Target.name + ". Pos: " + Target.transform.position.ToString());

            if (Vector3.Distance(Character.transform.position, this.Target.transform.position) <= rangeMin)
            {
                Debug.Log("CHASE(" + Character.gameObject.name + "). Reached Destination: " + Target.transform.position.ToString());
                return Result.Success;
            }

            else if (Vector3.Distance(Character.transform.position, this.Target.transform.position) >= rangeMax)
            {
                Debug.Log("CHASE(" + Character.gameObject.name + "). Target out of range: " + Target.transform.position.ToString());
                Character.StartPathfinding(Character.transform.position);
                return Result.Failure;
            }

            else
            {
                Debug.Log("CHASE(" + Character.gameObject.name + "). Still moving: " + Target.transform.position.ToString());
                Character.StartPathfinding(Target.transform.position);
                return Result.Running;
            }

        }
    }
}
