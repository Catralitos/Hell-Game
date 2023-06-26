using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    class SearchForTarget : Task // Go look for a target in a given spot
    {
        public NPC Character { get; set; }

        public GameObject SearchPoint { get; set; }
        public float rangeDestination { get; set; }

        public GameObject Target { get; set; }
        public float rangeTarget { get; set; }



        public SearchForTarget(Enemy character, GameObject target,
                                float _rangeTarget, GameObject searchPoint, float _rangeDestination)
        {
            this.Character = character;
            this.SearchPoint = searchPoint;
            this.Target = target;
            this.rangeTarget = _rangeTarget;
            this.rangeDestination = _rangeDestination;
        }

        public override Result Run() // Succeeds if target is found
        {
            if (Target == null)
                return Result.Failure;

            if (Vector3.Distance(Character.transform.position, this.Target.transform.position) <= rangeTarget)
            {
                Debug.Log("Found Target! : " + this.Target.transform.position.x + "|" + this.Target.transform.position.y + "|" + this.Target.transform.position.z);
                Character.StartPathfinding(Character.transform.position);
                return Result.Success;
            }

            else if (Vector3.Distance(Character.transform.position, this.SearchPoint.transform.position) <= rangeDestination)
            {
                Debug.Log("Reached Destination! : " + this.Character.transform.position.x + "|" + this.Character.transform.position.y + "|" + this.Character.transform.position.z);
                Character.StartPathfinding(Character.transform.position);
                return Result.Failure;
            }

            else
            {
                Character.StartPathfinding(SearchPoint.transform.position);
                return Result.Running;
            }

        }
    }
}