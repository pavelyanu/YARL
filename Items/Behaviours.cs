using YARL.Actors;
using YARL.Actions;

namespace YARL.Items
{
    public interface IPickBehaviour
    {
	public void Pick(Player player, Item item);
    }

    public interface IEquipBehaviour
    {
	public void Equip(Player player);
	public void UnEquip(Player player);
    }

    public interface IUseBehaviour
    {
	public void Use(Player player);
    }

    public class AddItemBehaviour : IPickBehaviour
    {
	public void Pick(Player player, Item item)
	{
	    player.inventory.Add(item);
	}
    }

    public class AddActionBehaviour : IEquipBehaviour
    {
	Action action;
	public AddActionBehaviour(Action a)
	{
	    action = a;
	}

	public void Equip(Player player)
	{
	    player.AddAction(action);
	}

	public void UnEquip(Player player)
	{
	    player.RemoveAction(action);
	}
    }

    public class DoNothing : IPickBehaviour, IEquipBehaviour, IUseBehaviour
    {
	public void Pick(Player player, Item item)
	{

	}

	public void Equip(Player player)
	{

	}

	public void UnEquip(Player player)
	{

	}

	public void Use(Player player)
	{

	}
    }

}

