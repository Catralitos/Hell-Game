using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AI;
using Characters.BehaviorTrees;

namespace Characters
{
    public enum NPCState
    {
        Idle = 0,
        Walk,
        Talk
    };

    public enum BehaviorTreeType
    {
        Main
    };

    public class NPC : MonoBehaviour
    {
        protected GameObject character;

        public BehaviorTreeType currBehaviorTreeType
        { get; set; }

        public Task MainBehaviourTree;

        public Task currBehaviorTree
        { get; set; }

        // Pathfinding
        public void ChangeBehaviourTree(Task newTree, BehaviorTreeType newTreeType)
        {
            this.currBehaviorTree = newTree;
            this.currBehaviorTreeType = newTreeType;
        }

        protected UnityEngine.AI.NavMeshAgent navMeshAgent;
        private Vector3 previousTarget;

        public NPCState npcState; //This is checked by conditions in the StateMachine
        public GameObject[] talkingTo;

        public void SetupNavmesh()
        {
            previousTarget = new Vector3(0.0f, 0.0f, 0.0f);
            this.character = this.gameObject;
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        public void StartPathfinding(Vector3 targetPosition)
        {
            //if the targetPosition received is the same as a previous target, then this a request for the same target
            //no need to redo the pathfinding search
            if (!this.previousTarget.Equals(targetPosition))
            {

                this.previousTarget = targetPosition;

                navMeshAgent.SetDestination(targetPosition);

            }
        }

        public void StopPathfinding()
        {
            navMeshAgent.isStopped = true;
        }

        public float GetDistanceToTarget(Vector3 originalPosition, Vector3 targetPosition)
        {
            var distance = 0.0f;

            NavMeshPath result = new NavMeshPath();
            //var r = navMeshAgent.CalculatePath(targetPosition, result);
            var r = NavMesh.CalculatePath(originalPosition, targetPosition, NavMesh.AllAreas, result);
            if (r == true)
            {
                var currentPosition = originalPosition;
                foreach (var c in result.corners)
                {
                    //Rough estimate, it does not account for shortcuts so we have to multiply it
                    distance += Vector3.Distance(currentPosition, c) * 0.65f;
                    currentPosition = c;
                }
                return distance;
            }

            //Default value
            return 100;
        }




        public void SwitchToWalkState()
        {
            StartCoroutine(WaitBeforeSwitch());
        }

        IEnumerator WaitBeforeSwitch()
        {
            int wait_time = Random.Range(0, 4);
            yield return new WaitForSeconds(wait_time);
            npcState = NPCState.Walk;
        }
    }
}