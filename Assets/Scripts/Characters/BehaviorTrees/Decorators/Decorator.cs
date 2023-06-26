using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees.Decorators
{
    public class Decorator : Task
    {
        public Task child { get; set; }

        public Decorator(Task child)
        {
            this.child = child;
        }

        public Decorator() { }
    }

}
