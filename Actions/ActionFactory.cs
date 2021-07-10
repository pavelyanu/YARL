using YARL.Items;
using YARL.Drawing;

namespace YARL.Actions
{
    class ActionFactory
    {

	ItemFactory itemFactory;

	public ActionFactory(ItemFactory _itemFactory)
	{
	    itemFactory = _itemFactory;
	}

	public Action CreateAttack(
	    int cost,
	    int numOfTargets,
	    string name,
	    int range,
	    bool str_based,
	    bool dex_based,
	    bool inte_based,
	    int dice,
	    int numberOfDice,
	    Item uses
	)
	{
	    var attack = new Attack
	    (
	    _cost: cost,
	    _numOfTargets: numOfTargets,
	    _name: name,
	    _range: range,
	    _str_based: str_based,
	    _dex_based: dex_based,
	    _inte_based: inte_based,
	    _dice: dice,
	    _numberOfDice: numberOfDice,
	    _uses: uses
	    );
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
		numberOfDice: 1,
		uses: null
	    );
	}

	public Action CreateRapireAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Rapire attack",
		range: 1,
		str_based: false,
		dex_based: true,
		inte_based: false,
		dice: 6,
		numberOfDice: 1,
		uses: null
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
		numberOfDice: 1,
		uses: null
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
		numberOfDice: 1,
		uses: null
	    );
	}

	public Action CreateGreatSwordAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Great sword attack",
		range: 1,
		str_based: true,
		dex_based: false,
		inte_based: false,
		dice: 6,
		numberOfDice: 2,
		uses: null
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
		numberOfDice: 1,
		uses: itemFactory.CreateArrow()
	    );
	}

	public Action CreateWarBowAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "War bow attack",
		range: 7,
		str_based: false,
		dex_based: true,
		inte_based: false,
		dice: 8,
		numberOfDice: 1,
		uses: itemFactory.CreateArrow()
	    );
	}

	public Action CreateBiteAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Bite attack",
		range: 1,
		str_based: true,
		dex_based: false,
		inte_based: false,
		dice: 8,
		numberOfDice: 1,
		uses: itemFactory.CreateArrow()
	    );
	}

	public Action CreateTalonAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Talon attack",
		range: 1,
		str_based: false,
		dex_based: true,
		inte_based: false,
		dice: 6,
		numberOfDice: 2,
		uses: itemFactory.CreateArrow()
	    );
	}
	
	public Action CreateSpellAttack()
	{
	    return CreateAttack(
		cost: 1,
		numOfTargets: 1,
		name: "Spell attack",
		range: 7,
		str_based: false,
		dex_based: false,
		inte_based: true,
		dice: 4,
		numberOfDice: 4,
		uses: itemFactory.CreateArrow()
	    );
	}

    }
}
