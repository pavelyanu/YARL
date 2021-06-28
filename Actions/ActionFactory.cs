namespace YARL.Actions
{
    class ActionFactory
    {

	public Action CreateAttack(
	    int cost,
	    int numOfTargets,
	    string name,
	    int range,
	    bool str_based,
	    bool dex_based,
	    bool inte_based,
	    int dice,
	    int numberOfDice
	)
	{
	    var attack = new Attack();
	    attack.cost = cost;
	    attack.numOfTargets = numOfTargets;
	    attack.name = name;
	    attack.range = range;
	    attack.str_based = str_based;
	    attack.dex_based = dex_based;
	    attack.inte_based = inte_based;
	    attack.dice = dice;
	    attack.numberOfDice = numberOfDice;
	    return attack;
	}

	public Action CreateDaggerAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Dagger attack",
		range: 1,
		str_based: false,
		dex_based: true,
		inte_based: false,
		dice: 4,
		numberOfDice: 1
	    );
	}

	public Action CreateShortSwordAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Short sword attack",
		range: 1,
		str_based: true,
		dex_based: false,
		inte_based: false,
		dice: 6,
		numberOfDice: 1
	    );
	}

	public Action CreateLongSwordAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Long sword attack",
		range: 1,
		str_based: true,
		dex_based: false,
		inte_based: false,
		dice: 8,
		numberOfDice: 1
	    );
	}

	public Action CreateBowAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Bow attack",
		range: 5,
		str_based: false,
		dex_based: true,
		inte_based: false,
		dice: 6,
		numberOfDice: 1
	    );
	}
    }
}
