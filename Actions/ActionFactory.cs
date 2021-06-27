namespace YARL.Actions
{
    class ActionFactory
    {

	public Action CreateAttack(
	    int cost,
	    int numOfTargets,
	    string name,
	    int range,
	    int attackModifier,
	    int damageModifier,
	    int dice,
	    int numberOfDice
	)
	{
	    var attack = new Attack();
	    attack.cost = cost;
	    attack.numOfTargets = numOfTargets;
	    attack.name = name;
	    attack.range = range;
	    attack.dice = dice;
	    attack.numberOfDice = numberOfDice;
	    return attack;
	}

	public Action CreateShortSwordAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Short sword attack",
		range: 1,
		attackModifier: 0,
		damageModifier: 0,
		dice: 6,
		numberOfDice: 1
	    );
	}
    }
}
