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
	
	public bool CanEquip(Player player)
	{
	    return equipBehaviour.CanEquip(player);
	}

	public string Equip(Player player)
	{
	    return equipBehaviour.Equip(player);
	}

	public string UnEquip(Player player)
	{
	    return equipBehaviour.UnEquip(player);
	}

	public bool CanUse(Player player)
	{
	    return useBehaviour.CanUse(player);
	}

	public string Use(Player player)
	{
	    return useBehaviour.Use(player);
	}

	
    }
}
