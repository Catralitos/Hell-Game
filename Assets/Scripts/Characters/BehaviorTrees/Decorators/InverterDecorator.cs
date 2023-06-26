using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees.Decorators
{
    public class InverterDecorator : Decorator
    {
        public InverterDecorator(Task child) : base(child)
        {
        }

        public InverterDecorator() { }

        public override Result Run()
        {
            Result result = this.child.Run();

            if (result.Equals(Result.Success))
                return Result.Failure;
            else if (result.Equals(Result.Failure))
                return Result.Success;
            else
                return result;
        }
    }

}

