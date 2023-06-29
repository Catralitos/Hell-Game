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

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
    
        private Animator _animator;
        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 move = ai.velocity;
            if (_animator != null)
            {
                _animator.SetFloat(Horizontal, move.x);
                _animator.SetFloat(Vertical, move.y);
                _animator.SetFloat(Speed, move.sqrMagnitude);
            }
        }
        
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