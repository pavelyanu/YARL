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
	public string Use(Player player);
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

    public class ArmourBehaviour : IEquipBehaviour
    {
	int ac;
	int cap;
	public ArmourBehaviour(int _ac, int _cap)
	{
	    ac = _ac;
	}

	public void Equip(Player player)
	{
	    player.AddArmour(ac, cap);
	}

	public void UnEquip(Player player)
	{
	    player.RemoveArmour();
	}
    }

    public class HealBehavour : IUseBehaviour
    {
	int healing;
	public HealBehavour(int _healing)
	{
	    healing = _healing;
	}
	public string Use(Player player)
	{
	    int result = player.health + healing;
	    if (result > player.maxHealth)
	    {
		result = player.maxHealth - player.health;
	    }
	    player.Inflict(-result);
	    return $"You have healed yourself up to {player.health}";
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

	public string Use(Player player)
	{
	    return "";
	}
    }

}

