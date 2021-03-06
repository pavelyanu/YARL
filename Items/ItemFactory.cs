using YARL.Actions;
using YARL.Drawing;

namespace YARL.Items
{
    class ItemFactory
    {
	public IDrawBehaviour drawBehaviour { get; set; }
	ActionFactory actionFactory;

	public ItemFactory(IDrawBehaviour _drawBehaviour)
	{
	    drawBehaviour = _drawBehaviour;
	    actionFactory = new ActionFactory(this);
	}

	public Item Create(
	    IDrawBehaviour drawBehaviour,
	    char glyph,
	    string name,
	    bool equipable,
	    bool usable,
	    int amount,
	    PossessionType possessionType,
	    EquipmentType equipmentType,
	    IPickBehaviour pickBehaviour,
	    IEquipBehaviour equipBehaviour, 
	    IUseBehaviour useBehaviour
	)
	{
	    Item item = new Item();
	    item.drawBehaviour = drawBehaviour;
	    item.glyph = glyph;
	    item.name = name;
	    item.equipable = equipable;
	    item.usable = usable;
	    item.amount = amount;
	    item.possessionType = possessionType;
	    item.equipmentType = equipmentType;
	    item.pickBehaviour = pickBehaviour;
	    item.equipBehaviour = equipBehaviour;
	    item.useBehaviour = useBehaviour;
	    return item;
	}

	public Item CreateDagger()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: ',',
		name: "Dagger",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateDaggerAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateRapire()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '_',
		name: "Rapire",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateRapireAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateShortSword()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '-',
		name: "Short Sword",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateShortSwordAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateLongSword()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '|',
		name: "Long Sword",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateLongSwordAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateGreatSword()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: ']',
		name: "Great Sword",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateGreatSwordAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateWarhammer()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '=',
		name: "Warhammer Sword",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateWarhammerAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateBow()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '^',
		name: "Bow",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateBowAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateLongBow()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: ';',
		name: "Long bow",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateLongBowAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateWarBow()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '}',
		name: "War bow",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateLongBowAttack()),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateArrow()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '/',
		name: "Arrow",
		equipable: false,
		usable: false,
		amount: 10,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.None,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new DoNothing(),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateLightArmor()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: ')',
		name: "Light armor",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Armor,
		equipmentType: EquipmentType.Body,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new ArmourBehaviour(1, -1, 0),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateMediumArmor()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '(',
		name: "Medium armor",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Armor,
		equipmentType: EquipmentType.Body,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new ArmourBehaviour(3, 2, 2),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateHeavyArmor()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '>',
		name: "Heavy armor",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Armor,
		equipmentType: EquipmentType.Body,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new ArmourBehaviour(5, 0, 4),
		useBehaviour: new DoNothing()
	    );
	}

	public Item CreateGoalGem()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '$',
		name: "Gem",
		equipable: false,
		usable: true,
		amount: 1,
		possessionType: PossessionType.Armor,
		equipmentType: EquipmentType.None,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new DoNothing(),
		useBehaviour: new TeleportBehaviour()
	    );
	}

	public Item CreateHealingPotion()
	{
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '!',
		name: "Healing potion",
		equipable: false,
		usable: true,
		amount: 1,
		possessionType: PossessionType.Armor,
		equipmentType: EquipmentType.None,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new DoNothing(),
		useBehaviour: new HealBehavour(5)
	    );
	}
    }
}
