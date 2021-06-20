using System.Collections.Generic;
using System;
using YARL.Actors;

namespace YARL.Actions
{
    public abstract class Action : IEquatable<Action>
    {
	public abstract int cost { get; }
	public abstract int numOfTargets { get; }
	public abstract string name { get; }
	public abstract int range { get; }
	public abstract void Do(List<Entity> targets);

	public override bool Equals(object obj)
	{
	    return Equals(obj as Action);
	}

	public bool Equals(Action a)
	{
	    return a != null &&
		a.name == this.name;
	}

	public override int GetHashCode()
	{
	    return name.GetHashCode();
	}
    }
}
