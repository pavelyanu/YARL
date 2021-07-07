using System.Collections.Generic;
using System;
using YARL.Actors;
using YARL.Core;
using YARL.Items;

namespace YARL.Actions
{
    public abstract class Action : IEquatable<Action>
    {
	public int cost { get; set; }
	public int numOfTargets { get; set; }
	public string name { get; set; }
	public int range { get; set; }
	public Item uses { get; set; }
	public abstract string Do(List<Entity> targets, Entity actor, Level level);

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
