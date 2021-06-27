using System.Collections.Generic;
using YARL.Items;
using YARL.Actions;
using YARL.Drawing;
using YARL.Core;

namespace YARL.Actors
{
    class EntityFactory
    {

	IDrawBehaviour drawBehaviour;
	ItemFactory itemFactory;
	ActionFactory actionFactory;

	public EntityFactory(IDrawBehaviour _drawBehaviour)
	{
	    drawBehaviour = _drawBehaviour;
	    itemFactory = new ItemFactory(drawBehaviour);
	    actionFactory = new ActionFactory();
	}

	public Entity CreateMonster(
	    char glyph,
	    IDrawBehaviour drawBehaviour,
	    int movement,
	    string name,
	    int str,
	    int dex,
	    int inte,
	    int health,
	    List<Item> loot,
	    double lootChance,
	    Action action
	)
	{
	    return new Monster(
		_glyph: glyph,
		_drawBehaviour: drawBehaviour,
		_movement: movement,
		_name: name,
		_str: str,
		_dex: dex,
		_inte: inte,
		_health: health,
		_loot: loot,
		_lootchance: lootChance,
		_action: action
	    );
	}

	public Player CreatePlayer()
	{
	    return new Player(
		_drawBehaviour: drawBehaviour,
		_str: Roller.Roll(6, 3),
		_dex: Roller.Roll(6, 3),
		_inte: Roller.Roll(6, 3),
		_health: 8
	    );
	}

	public Entity CreateGoblin()
	{
	    var loot = new List<Item>();
	    loot.Add(itemFactory.CreateShortSword());
	    return CreateMonster(
		glyph: 'g',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Goblin",
		str: -2,
		dex: 0,
		inte: -3,
		health: 5,
		loot: loot,
		lootChance: 0.1,
		action: actionFactory.CreateShortSwordAttack()
	    );
 	}

    }
}
