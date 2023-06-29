using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using Events.ScriptableObjects;
using Management;
using Random = UnityEngine.Random;
using Pathfinding;
using CharacterPathfinding;

namespace Characters
{
    public enum NPCState
    {
        Idle = 0,
        Walk,
        Talk
    };

    public class NPC : PathfindingEntity
    {

        [SerializeField] public List<TimedPathfindingPair> targetsByTime;
        [Header("Listening To")] public TimeChannelSO hourPassedEvent;
        
        protected GameObject character;

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
            foreach (TimedPathfindingPair pair in targetsByTime.Where(pair => pair.time.day == time.day && pair.time.hour == time.hour))
            {
                AIDestinationSetter.target = pair.transform;
            }
        }
        
        public NPCState npcState; //This is checked by conditions in the StateMachine
        public GameObject[] talkingTo;

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