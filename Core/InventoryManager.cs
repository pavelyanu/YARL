using System.Text;
using System.Collections.Generic;
using System.Linq;
using YARL.Items;
using YARL.Actors;

namespace YARL.Core
{
    class InventoryManager
    {
	Player player;
	StringBuilder sb; 
	Equipable hand;

	public InventoryManager(Player p)
	{
	    player = p;
	    sb = new StringBuilder();
	    hand = null;
	}

	public string Draw()
	{
	    sb.Clear();
	    if (hand is not null)
		sb.AppendLine($"{hand.name} is in the hand");
	    foreach(var item in player.possessions.Keys)
	    {
		sb.Append(item);
		int count = player.possessions[item].Count;
		if (count > 1)
		    sb.Append($" {count}");
		sb.AppendLine();
	    }
	    return sb.ToString();
	}

	public void Equip()
	{
	    if (player.possessions.Count != 0)
	    {
		player.possessions.Values.ToList()[0][0].Equip(player);
		hand = player.possessions.Values.ToList()[0][0];
		player.possessions.Remove(hand.name);
	    }
	}

    }
}
