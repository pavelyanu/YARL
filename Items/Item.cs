using YARL.Actors;
using YARL.Drawing;

namespace YARL.Items
{

    public enum EquipmentType
    {
	Hands,
	Body,
	None
    }

    public enum PossessionType
    {
	Weapon,
	Armor,
	Book
    }

    public class Item : IDrawable
    {
	public IDrawBehaviour drawBehaviour { get; set; }
	public char glyph { get; set; }
	public string name { get; set; }
	public bool equipable { get; set; }
	public bool usable { get; set; }
	public int amount { get; set; }
	public PossessionType possessionType { get; set; }
	public EquipmentType equipmentType { get; set; }
	public IPickBehaviour pickBehaviour { get; set; }
	public IEquipBehaviour equipBehaviour { get; set; }
	public IUseBehaviour useBehaviour { get; set; }

	public char Draw()
	{
	    return drawBehaviour.Draw(glyph);
	}

	public void Pick(Player player)
	{
	    pickBehaviour.Pick(player, this);
	}

	public void Equip(Player player)
	{
	    equipBehaviour.Equip(player);
	}

	public void UnEquip(Player player)
	{
	    equipBehaviour.UnEquip(player);
	}

	public string Use(Player player)
	{
	    return useBehaviour.Use(player);
	}

	
    }
}
