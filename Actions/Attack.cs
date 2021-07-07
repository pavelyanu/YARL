using System.Text;
using System.Collections.Generic;
using YARL.Actors;
using YARL.Core;
using YARL.Items;

namespace YARL.Actions
{
    class Attack : Action
    {
	StringBuilder sb;

	public int dice { get; set; }
	public int numberOfDice { get; set; }
	public bool str_based { get; set; }
	public bool dex_based { get; set; }
	public bool inte_based { get; set; }

	
	public Attack (
	    int _cost,
	    int _numOfTargets,
	    string _name,
	    int _range,
	    bool _str_based,
	    bool _dex_based,
	    bool _inte_based,
	    int _dice,
	    int _numberOfDice,
	    Item _uses
	)
	{
	    cost = _cost;
	    numOfTargets = _numOfTargets;
	    name = _name;
	    range = _range;
	    str_based = _str_based;
	    dex_based = _dex_based;
	    inte_based = _inte_based;
	    dice = _dice;
	    numberOfDice = _numberOfDice;
	    uses = _uses;
	}

	public override string Do(List<Entity> targets, Entity actor, Level level)
	{
	    if (uses is not null)
	    {
		if (actor.inventory.items.ContainsKey(uses.name))
		{
		    actor.inventory.Remove(uses);
		    return _Do(targets, actor, level);
		} else 
		{
		    string have;
		    if (actor.name == "You")
			have = "have";
		    else have = "has";
		    return $"{actor.name} {have} no {uses.name} to make this action";
		}
	    } else
	    {
		return _Do(targets, actor, level);
	    }
	}

	public string _Do(List<Entity> targets, Entity actor, Level level)
	{
	    int modifier = 0;
	    if (str_based)
		modifier += actor.str;
	    if (dex_based)
		modifier += actor.dex;
	    if (inte_based)
		modifier += actor.inte;

	    int modification = 0;
	    sb = new StringBuilder();
	    foreach (var target in targets)
	    {
		if (range > 2 && level.GetDistance(actor.position, target.position) == 1)
		{
		    modification = -1;
		}
		string have;
		if (actor.name == "You")
		    have = "have";
		else have = "has";
		int roll = Roller.RollWithModification(20, modification);
		if (roll + modifier > target.armor_class)
		{
		    int damage = Roller.Roll(dice, numberOfDice) + modifier * numberOfDice;
		    target.Inflict(damage);
		    sb.AppendLine($"{actor.name} { have } hit {target.name} and dealt {damage} damage");
		} else {
		    sb.AppendLine($"{actor.name} { have } missed you {target.name}");
		}
	    }
	    sb.Remove(sb.Length - 1, 1);
	    return sb.ToString();
	}
    }
}
