using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees
{
    public class Sequence : CompositeTask
    {
        public Sequence(List<Task> tasks) : base(tasks)
        {
        }

        public Sequence() { }

        // A Sequence will return immediately with a failure status code when one of its children
        // fails. As long as its children are succeeding, it will keep going.If it runs out of children, it will
        // return in success
        public override Result Run()
        {

            if (this.currentChild < children.Count)
            {
                Result result = children[currentChild].Run();

                if (result == Result.Running)
                    return Result.Running;

                else if (result == Result.Failure)
                {
                    currentChild = 0;
                    return Result.Failure;
                }
                else
                {
                    currentChild++;
                    if (children.Count > this.currentChild)
                        return Result.Running;
                    else
                    {
                        currentChild = 0;
                        return Result.Success;
                    }
                }
            }
            return Result.Success;
        }
    }
}
