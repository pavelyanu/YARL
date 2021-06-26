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

	public int attackModifier { get; set; }
	public int damageModifier { get; set; }
	public int dice { get; set; }
	public int numberOfDice { get; set; }

	public override string Do(List<Entity> targets)
	{
	    sb = new StringBuilder();
	    foreach (var target in targets)
	    {
		if (Roller.Roll(20) + attackModifier > target.armor_class)
		{
		    int damage = Roller.Roll(dice, numberOfDice) + damageModifier * numberOfDice;
		    target.Inflict(damage);
		    sb.AppendLine($"You have hit {target.name} and dealt {damage} damage");
		} else {
		    sb.AppendLine($"You have not managed to hit {target.name}");
		}
	    }
	    return sb.ToString();
	}
    }
}
