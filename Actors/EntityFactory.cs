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
		_str: 2,
		_dex: 2,
		_inte: 2,
		_health: 8
	    );
	}

	public Entity CreateGoblin()
	{
	    var loot = new List<Item>();
	    loot.Add(itemFactory.CreateShortSword());
	    loot.Add(itemFactory.CreateLightArmour());
	    return CreateMonster(
		glyph: 'g',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Goblin",
		str: 0,
		dex: 0,
		inte: -3,
		health: 5,
		loot: loot,
		lootChance: 0.7,
		action: actionFactory.CreateShortSwordAttack()
	    );
 	}

	public Entity CreateGoblinWithBow()
	{
	    var loot = new List<Item>();
	    loot.Add(itemFactory.CreateLightArmour());
	    return CreateMonster(
		glyph: 'b',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Goblin with bow",
		str: 0,
		dex: 1,
		inte: -3,
		health: 4,
		loot: loot,
		lootChance: 0.7,
		action: actionFactory.CreateBowAttack()
	    );
 	}

	public Entity CreateOrk()
	{
	    var loot = new List<Item>();
	    loot.Add(itemFactory.CreateLongSword());
	    return CreateMonster(
		glyph: 'o',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Ork",
		str: 2,
		dex: 0,
		inte: -3,
		health: 4,
		loot: loot,
		lootChance: 0.9,
		action: actionFactory.CreateLongSwordAttack()
	    );
 	}
    }
}
