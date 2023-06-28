using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using Characters.BehaviorTrees;
using Events.ScriptableObjects;
using Management;
using Random = UnityEngine.Random;

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
        [Serializable]
        public class Pair
        {
            public TimeStep time;
            public Transform transform;
        }

        [SerializeField] public List<Pair> targetsByTime;
        [Header("Listening To")] public TimeChannelSO hourPassedEvent;
        private Transform _currentTarget;
        
        protected GameObject character;

        public BehaviorTreeType currBehaviorTreeType
        { get; set; }

        public Task MainBehaviourTree;

        public Task currBehaviorTree
        { get; set; }

        private void OnEnable()
        {
            hourPassedEvent.OnEventRaised += CheckNewTarget;
        }
        
        private void OnDisable()
        {
            hourPassedEvent.OnEventRaised -= CheckNewTarget;
        }

        private void CheckNewTarget(TimeStep time)
        {
            foreach (Pair pair in targetsByTime.Where(pair => pair.time.day == time.day && pair.time.hour == time.hour))
            {
                _currentTarget = pair.transform;
            }
        }
        

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
            // DO NOTHING
        }

        public void DoNotSetupNavmesh()
        {
            previousTarget = new Vector3(0.0f, 0.0f, 0.0f);
            this.character = this.gameObject;
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        public void StartPathfinding(Vector3 targetPosition)
        {
            // DO NOTHING
        }

        public void DoNotStartPathfinding(Vector3 targetPosition)
        {
            if (targetPosition == null)
            {
                Debug.Log("Target position does not exist");
                return;
            }

            //if the targetPosition received is the same as a previous target, then this a request for the same target
            //no need to redo the pathfinding search
            if (!this.previousTarget.Equals(targetPosition))
            {
                //Debug.Log("Prev Target: " + previousTarget.ToString() + "New target: " + targetPosition.ToString());

                this.previousTarget = targetPosition;

                navMeshAgent.SetDestination(targetPosition);

            }
        }

        public void StopPathfinding()
        {
            // DO NOTHING
        }

        public void DoNotStopPathfinding()
        {
            navMeshAgent.isStopped = true;
        }

        public float GetDistanceToTarget(Vector3 originalPosition, Vector3 targetPosition)
        {
            // DO NOTHING
            return 50.0f;
        }


        public float DoNotGetDistanceToTarget(Vector3 originalPosition, Vector3 targetPosition)
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