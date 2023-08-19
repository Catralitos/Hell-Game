using System;
using Management;
using UnityEngine;

namespace Characters.CharacterPathfinding
{
    [Serializable]
    public class TimedPathfindingPair
    {
        public TimeStep time;
        public Transform transform;
    }
}