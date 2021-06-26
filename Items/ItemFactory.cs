using YARL.Actions;
using YARL.Drawing;

namespace YARL.Items
{
    class ItemFactory
    {
	public IDrawBehaviour drawBehaviour { get; set; }
	public ItemFactory()
	{
	    drawBehaviour = new DefaultDraw();
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

	public Item CreateShortSword()
	{
	    var attack = new Attack();
	    attack.cost = 1;
	    attack.numOfTargets = 1;
	    attack.name = "Shor sword attack";
	    attack.range = 1;
	    attack.attackModifier = 1;
	    attack.damageModifier = 1;
	    attack.dice = 6;
	    attack.numberOfDice = 1;
	    
	    return Create(
		drawBehaviour: drawBehaviour,
		glyph: '-',
		name: "Shot Sword",
		equipable: true,
		usable: false,
		amount: 1,
		possessionType: PossessionType.Weapon,
		equipmentType: EquipmentType.Hands,
		pickBehaviour: new AddItemBehaviour(),
		equipBehaviour: new AddActionBehaviour(attack),
		useBehaviour: new DoNothing()
	    );

	}
    }
}
