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
	    actionFactory = new ActionFactory();
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
		equipBehaviour: new AddActionBehaviour(actionFactory.CreateShortSwordAttack()),
		useBehaviour: new DoNothing()
	    );

	}
    }
}
