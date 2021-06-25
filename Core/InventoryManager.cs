using System.Text;
using System.Collections.Generic;
using YARL.Items;
using YARL.Actors;

namespace YARL.Core
{
    public enum State
    {
	SelectingEquipment,
	SelectingUsables,
	Overview
    }

    class InventoryManager
    {
	Inventory inventory;
	StringBuilder sb; 
	Dictionary<char, Item> chooseMap;
	State state;	

	public InventoryManager(Player p)
	{
	    inventory = p.inventory;
	    sb = new StringBuilder();
	    state = State.Overview;
	    chooseMap = null;
	}

	public void ProcessInput(char key)
	{
	    if (state == State.Overview)
	    {
		switch (key)
		{
		    case 'e':
			chooseMap = GetChooseMap(inventory.GetEquipable());
			if (chooseMap is not null)
			{
			    state = State.SelectingEquipment;
			    break;
			} else break;
		}
	    } else if (state == State.SelectingEquipment)
	    {
		if (chooseMap.ContainsKey(key))
		{
		    inventory.EquipItem(chooseMap[key]);	    
		    state = State.Overview;
		}
	    }
	}

	Dictionary<char, Item> GetChooseMap(List<Item> items)
	{
	    if (items.Count == 0)
		return null;
	    Dictionary<char, Item> result = new Dictionary<char, Item>();
	    for (int i = 97; i < 123; i++)
	    {
		result[(char) i] = items[0];
		items.RemoveAt(0);
		if (items.Count == 0)
		    break;
	    }
	    return result;
	}

	public string Draw()
	{
	    sb.Clear();
	    switch (state)
	    {
		case State.Overview:
		    foreach(var item in inventory.GetEquipped())
		    {
			sb.AppendLine($"You have {item.name} equipped");
		    }
		    foreach(var item in inventory.items.Keys)
		    {
			sb.AppendLine(item);
		    }
		    return sb.ToString();
		case State.SelectingEquipment:
		    foreach(var item in chooseMap)
		    {
			sb.AppendLine($"{item.Key} - {item.Value}");
		    }
		    return sb.ToString();
		default:
		    return "Error";
	    }
	}
    }
}
