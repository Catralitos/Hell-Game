using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees
{
    public class Task
    {
        public enum Result { Running, Failure, Success }
        public Task() { }

        public virtual Result Run()
        {
            return Result.Failure;
        }
    }
}
