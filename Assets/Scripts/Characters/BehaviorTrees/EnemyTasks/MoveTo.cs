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

            Debug.Log("MOVE_TO(" + Character.gameObject.name + "). Target = " + Target.name + ". Pos: " + Target.transform.position.ToString());


            if (Vector3.Distance(Character.transform.position, this.Target.transform.position) <= range)
            {
                Debug.Log("MOVE_TO(" + Character.gameObject.name + ").Reached Destination: " + Target.transform.position.ToString());
                return Result.Success;
            }
            else
            {
                Debug.Log("MOVE_TO(" + Character.gameObject.name + "). Still moving: " + Target.transform.position.ToString());
                Character.StartPathfinding(Target.transform.position);
                return Result.Running;
            }

        }

    }

}