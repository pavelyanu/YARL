using System.Text;
using System.Collections.Generic;
using YARL.Actors;
using YARL.Core;

namespace YARL.Actions
{
    class Attack : Action
    {
	public override int cost { get; set; }
	public override int numOfTargets { get; set; }
	public override string name { get; set; }
	public override int range { get; set; }
	
	StringBuilder sb;

	public int dice { get; set; }
	public int numberOfDice { get; set; }
	public bool str_based { get; set; }
	public bool dex_based { get; set; }
	public bool inte_based { get; set; }

	public override string Do(List<Entity> targets, Entity actor)
	{
	    int modifier = 0;
	    if (str_based)
		modifier += actor.str;
	    if (dex_based)
		modifier += actor.dex;
	    if (inte_based)
		modifier += actor.inte;

	    sb = new StringBuilder();
	    foreach (var target in targets)
	    {
		string have;
		if (actor.name == "You")
		    have = "have";
		else have = "has";
		if (Roller.Roll(20) + modifier > target.armor_class)
		{
		    int damage = Roller.Roll(dice, numberOfDice) + modifier * numberOfDice;
		    target.Inflict(damage);
		    sb.AppendLine($"{actor.name} { have } hit {target.name} and dealt {damage} damage");
		} else {
		    sb.AppendLine($"{actor.name} { have } missed you {target.name}");
		}
	    }
	    return sb.ToString();
	}
    }
}
