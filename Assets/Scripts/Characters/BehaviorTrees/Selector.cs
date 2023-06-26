using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees
{

    public class Selector : CompositeTask
    {
        public Selector(List<Task> tasks) : base(tasks)
        {
        }

        public Selector() { }

        // A Selector will return immediately with a success status code when one of its children
        // runs successfully.As long as its children are failing, it will keep on trying. If it runs out of
        // children completely, it will return a failure status code
        public override Result Run()
        {
            while (this.currentChild < children.Count)
            {
                Result result = children[currentChild].Run();

                if (result == Result.Running)
                    return Result.Running;

                else if (result == Result.Failure)
                {
                    currentChild++;
                    continue;
                }

                else
                {
                    currentChild = 0;
                    return Result.Success;
                }
            }

            currentChild = 0;
            return Result.Failure;

        }
    }
}