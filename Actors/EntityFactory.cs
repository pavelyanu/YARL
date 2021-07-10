using System.Collections.Generic;
using YARL.Items;
using YARL.Actions;
using YARL.Drawing;

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
	    actionFactory = new ActionFactory(itemFactory);
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
	    int exp,
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
		_exp: exp,
		_lootchance: lootChance,
		_action: action
	    );
	}

	public Player CreatePlayer()
	{
	    return new Player(
		_drawBehaviour: drawBehaviour,
		_str: 0,
		_dex: 0,
		_inte: 0,
		_health: 8
	    );
	}

	public Entity CreateGoblin()
	{
	    var goblin =  CreateMonster(
		glyph: 'g',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Goblin",
		str: 0,
		dex: 0,
		inte: -3,
		health: 5,
		exp: 5,
		lootChance: 0.7,
		action: actionFactory.CreateShortSwordAttack()
	    );
	    goblin.inventory.Add(itemFactory.CreateShortSword());
	    goblin.inventory.Add(itemFactory.CreateLightArmor());
	    return goblin;
 	}

	public Entity CreateGoblinWithBow()
	{
	    var goblin =  CreateMonster(
		glyph: 'b',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Goblin with bow",
		str: 0,
		dex: 1,
		inte: -3,
		health: 4,
		exp: 5,
		lootChance: 0.7,
		action: actionFactory.CreateBowAttack()
	    );
	    goblin.inventory.Add(itemFactory.CreateBow());
	    goblin.inventory.Add(itemFactory.CreateArrow());
	    return goblin;
 	}

	public Entity CreateOrc()
	{
	    var orc =  CreateMonster(
		glyph: 'o',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Orc",
		str: 2,
		dex: 0,
		inte: -3,
		health: 8,
		exp: 5,
		lootChance: 1,
		action: actionFactory.CreateLongSwordAttack()
	    );
	    orc.inventory.Add(itemFactory.CreateLongSword());
	    return orc;
 	}

	public Entity CreateZombie()
	{
	    var zombie =  CreateMonster(
		glyph: 'z',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Zombie",
		str: 0,
		dex: -4,
		inte: -3,
		health: 15,
		exp: 10,
		lootChance: 0.5,
		action: actionFactory.CreateBiteAttack()
	    );
	    zombie.inventory.Add(itemFactory.CreateMediumArmor());
	    return zombie;
	}

	public Entity CreateGhoul()
	{
	    var ghoul =  CreateMonster(
		glyph: 'U',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Ghoul",
		str: 4,
		dex: 4,
		inte: -3,
		health: 8,
		exp: 20,
		lootChance: 0.7,
		action: actionFactory.CreateTalonAttack()
	    );
	    ghoul.inventory.Add(itemFactory.CreateGreatSword());
	    ghoul.inventory.Add(itemFactory.CreateMediumArmor());
	    return ghoul;
	}

	public Entity CreateNecromancer()
	{
	    var zombie =  CreateMonster(
		glyph: 'N',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Necromancer",
		str: 0,
		dex: 0,
		inte: 4,
		health: 18,
		exp: 50,
		lootChance: 0.9,
		action: actionFactory.CreateSpellAttack()
	    );
	    return zombie;
	}
    }
}
