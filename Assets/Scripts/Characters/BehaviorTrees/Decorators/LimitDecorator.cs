using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees.Decorators
{
    public class LimitDecorator : Decorator
    {
        public float RunLimit { get; set; }

        public float RunSoFar { get; set; }

        public LimitDecorator(Task child, float runLimit, float runSoFar) : base(child)
        {
            this.RunLimit = runLimit;
            this.RunSoFar = runSoFar;
        }

        public LimitDecorator() { }

        public override Result Run()
        {
            if (RunSoFar >= RunLimit)
                return Result.Failure;

            RunSoFar++;
            return this.child.Run();
        }
    }

}

