using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Characters.BehaviorTrees.EnemyTasks
{
    public class IsCharacterNearTarget : Task
    {
        protected NPC Character { get; set; }

        public GameObject Target { get; set; }

        public float range;

        public IsCharacterNearTarget(NPC character, GameObject target, float _range)
        {
            this.Character = character;
            this.Target = target;
            range = _range;
        }

        public override Result Run()
        {
            if (Vector3.Distance(Character.gameObject.transform.position, this.Target.transform.position) <= range)
            {
                Debug.Log("Character NEAR target!");
                return Result.Success;
            }
            else
            {
                Debug.Log("Character FAR from target!");
                return Result.Failure;
            }
        }
    }

}