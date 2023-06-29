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

namespace CharacterPathfinding
{
    [Serializable]
    public class TimedPathfindingPair
    {
        public TimeStep time;
        public Transform transform;
    }
}