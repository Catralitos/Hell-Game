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

namespace Characters
{
    public class PathfindingEntity: MonoBehaviour
    {
        protected AIDestinationSetter AIDestinationSetter;
        protected IAstarAI ai;
        
        protected GameObject entity;

        public PathfindingEntity() {}

        public virtual void Start()
        {
            this.entity = GetComponent<GameObject>();
            this.AIDestinationSetter = GetComponentInParent<AIDestinationSetter>();
            this.ai = this.AIDestinationSetter.GetComponent<IAstarAI>();
        }
    }
}