using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.BehaviorTrees.Decorators
{
	public class UntilFailDecorator : Decorator
	{
		public UntilFailDecorator(Task child) : base(child)
		{
		}

		public UntilFailDecorator() { }

		public override Result Run()
		{
			while (true)
			{
				Result result = this.child.Run();
				if (result.Equals(Result.Failure))
					break;
			}
			return Result.Success;
		}
	}

}
