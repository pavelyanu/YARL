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
		exp: 3,
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
		exp: 3,
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
		str: 1,
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
	
	public Entity CreateSkeleton()
	{
	    var skeleton = CreateMonster(
		glyph: 's',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Skeleton",
		str: 0,
		dex: 3,
		inte: -3,
		health: 8,
		exp: 10,
		lootChance: 0.5,
		action: actionFactory.CreateLongBowAttack()
	    );
	    skeleton.inventory.Add(itemFactory.CreateLongBow());
	    return skeleton;
	}
	
	public Entity CreateGhoul()
	{
	    var ghoul =  CreateMonster(
		glyph: 'U',
		drawBehaviour: drawBehaviour,
		movement: 7,
		name: "Ghoul",
		str: 4,
		dex: 4,
		inte: -3,
		health: 10,
		exp: 20,
		lootChance: 0.7,
		action: actionFactory.CreateTalonAttack()
	    );
	    ghoul.inventory.Add(itemFactory.CreateGreatSword());
	    ghoul.inventory.Add(itemFactory.CreateArrow());
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
		action: actionFactory.CreateMagicMissle()
	    );
	    return zombie;
	}

	public Entity CreateGnome()
	{
	    var gnome =  CreateMonster(
		glyph: 'G',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Gnome",
		str: 6,
		dex: 0,
		inte: -3,
		health: 20,
		exp: 80,
		lootChance: 0.7,
		action: actionFactory.CreateWarhammerAttack()
	    );
	    gnome.inventory.Add(itemFactory.CreateWarhammer());
	    gnome.inventory.Add(itemFactory.CreateHeavyArmor());
	    return gnome;
	}

	public Entity CreateArcherGnome()
	{
	    var gnome =  CreateMonster(
		glyph: 'A',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Archer gnome",
		str: 0,
		dex: 6,
		inte: -3,
		health: 15,
		exp: 80,
		lootChance: 0.7,
		action: actionFactory.CreateWarBowAttack()
	    );
	    gnome.inventory.Add(itemFactory.CreateWarBow());
	    gnome.inventory.Add(itemFactory.CreateArrow());
	    return gnome;
	}

	public Entity CreateGnomeKing()
	{
	    var gnome =  CreateMonster(
		glyph: 'K',
		drawBehaviour: drawBehaviour,
		movement: 5,
		name: "Gnome King",
		str: 8,
		dex: 3,
		inte: -3,
		health: 40,
		exp: 100,
		lootChance: 1,
		action: actionFactory.CreateWarhammerAttack()
	    );
	    return gnome;
	}

    }
}
