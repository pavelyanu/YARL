using System.Collections.Generic;
using System;
using YARL.Actors;

namespace YARL.Actions
{
    public abstract class Action : IEquatable<Action>
    {
	public abstract int cost { get; set; }
	public abstract int numOfTargets { get; set; }
	public abstract string name { get; set; }
	public abstract int range { get; set; }
	public abstract string Do(List<Entity> targets, Entity actor);

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
