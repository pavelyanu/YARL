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
	public bool CanEquip(Player player);
	public string Equip(Player player);
	public string UnEquip(Player player);
    }

    public interface IUseBehaviour
    {
	public bool CanUse(Player player);
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

	public bool CanEquip(Player player) => true;

	public string Equip(Player player)
	{
	    player.AddAction(action);
	    return $"Upon equipping this item you realize that now you can make {action.name}";
	}

	public string UnEquip(Player player)
	{
	    player.RemoveAction(action);
	    return $"Upon unequipping this item you realize that now you can't make {action.name}";
	}
    }

    public class ArmourBehaviour : IEquipBehaviour
    {
	int ac;
	int cap;
	int reqStr;

	public ArmourBehaviour(int _ac, int _cap, int _reqStr)
	{
	    ac = _ac;
	    cap = _cap;
	    reqStr = _reqStr;
	}

	public bool CanEquip(Player player)
	{
	    if (player.str >= reqStr)
	    {
		return true;
	    }
	    return false;
	}

	public string Equip(Player player)
	{
	    if (CanEquip(player))
	    {
		player.AddArmour(ac, cap);
		return $"You put the armor on";
	    } else 
	    {
		return $"You are not strong enough to put this armor on. The minimal str. is {reqStr}";
	    }
	}

	public string UnEquip(Player player)
	{
	    player.RemoveArmour();
	    return "You take the armor off";
	}
    }

    public class HealBehavour : IUseBehaviour
    {
	int healing;
	public HealBehavour(int _healing)
	{
	    healing = _healing;
	}
	
	public bool CanUse(Player player) => false;
	
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

    public class TeleportBehaviour : IUseBehaviour
    {
	public bool CanUse(Player player) => true;
	public string Use(Player player)
	{
	    player.LevelTransfer();
	    return "You attempet to use teleportation stone";
	}
    }

    public class DoNothing : IPickBehaviour, IEquipBehaviour, IUseBehaviour
    {
	public bool CanEquip(Player player) => false;
	public bool CanUse(Player player) => false;

	public void Pick(Player player, Item item)
	{

	}

	public string Equip(Player player)
	{
	    return "";
	}

	public string UnEquip(Player player)
	{
	    return "";
	}

	public string Use(Player player)
	{
	    return "";
	}
    }

}

