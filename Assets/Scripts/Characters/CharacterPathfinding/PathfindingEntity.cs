using Pathfinding;
using UnityEngine;

namespace Characters.CharacterPathfinding
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