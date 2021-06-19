using System.Numerics;
using YARL.Actors;
using YARL.Actions;

namespace YARL.Items
{
    public abstract class Pickable
    {
	public Vector2 position { get; protected set; }
	public string name { get; protected set; }
    }

    public abstract class Possessable
    {
	public string name { get; protected set; }
	public Entity possessedBy { get; protected set; }
	public Action action { get; protected set; }
	public Action GetAction()
	{
	    return action;
	}
    }

    public abstract class Equipable
    {
	public bool equipped { get; protected set; }
	public IEquipBehaviour behaviour { get; protected set; }
	public void Equip(Player player)
	{
	    behaviour.Equip(player, this);
	}
	public void UnEquip(Player player)
	{
	    behaviour.UnEquip(player, this);
	}
    }
}
