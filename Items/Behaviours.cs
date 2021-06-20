using YARL.Actors;
using YARL.Actions;

namespace YARL.Items
{
    public interface IPickBehaviour
    {
	public void Pick(Player player, Pickable self);
    }

    public interface IEquipBehaviour
    {
	public void Equip(Player player, Equipable self);
	public void UnEquip(Player player, Equipable self);
    }

    public class PossessablePickBehaviour : IPickBehaviour
    {
	Equipable possession;
	public PossessablePickBehaviour(Equipable item)
	{
	    possession = item;
	}
	
	public void Pick(Player player, Pickable pickable)
	{
	    player.AddPossession(possession);
	}
    }

    public class AddActionBehaviour : IEquipBehaviour
    {
	Action action;
	public AddActionBehaviour(Action a)
	{
	    action = a;
	}

	public void Equip(Player player, Equipable self)
	{
	    player.AddAction(action);
	}

	public void UnEquip(Player player, Equipable self)
	{
	    player.RemoveAction(action);
	}
    }

}

