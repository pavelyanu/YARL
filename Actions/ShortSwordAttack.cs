using YARL.Actors;
using System.Collections.Generic;

namespace YARL.Actions
{
    class ShortSwordAttack : Action
    {
	public override int cost { get => 1; }
	public override int numOfTargets { get => 1; }
	public override string name { get => "Short sword attack"; }
	public override int range { get => 1; }
	public override void Do(List<Entity> targets)
	{
	    foreach (var target in targets)
	    {
		target.Inflict(5);
	    }
	}
    }
}
