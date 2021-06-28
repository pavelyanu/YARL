using System.Text;
using System.Collections.Generic;
using Serilog;
using YARL.Items;
using YARL.Actors;

namespace YARL.Core
{
    public enum State
    {
	SelectingEquipment,
	SelectingEquipped,
	SelectingUsables,
	Overview
    }

    class InventoryManager
    {
	Inventory inventory;
	Dictionary<char, Item> chooseMap;
	string bottomMessage;
	State state;	
	public bool selecting { get => 
				state == State.SelectingEquipment ||
				state == State.SelectingEquipped ||
				state == State.SelectingUsables; }

	public InventoryManager(Player p)
	{
	    inventory = p.inventory;
	    bottomMessage = "";
	    state = State.Overview;
	    chooseMap = null;
	}

	public void ProcessInput(char key)
	{
	    if (state == State.Overview)
	    {
		bottomMessage = "";
		switch (key)
		{
		    case 'e':
			chooseMap = GetChooseMap(inventory.GetEquipable());
			if (chooseMap is not null)
			{
			    state = State.SelectingEquipment;
			}
			break;
		    case 'r':
			chooseMap = GetChooseMap(inventory.GetEquipped());
			if (chooseMap is not null)
			{
			    state = State.SelectingEquipped;
			}
			break;
		    case 'u':
			chooseMap = GetChooseMap(inventory.GetUsable());
			if (chooseMap is not null)
			{
			    state = State.SelectingUsables;
			}
			break;
		}
	    } else if (state == State.SelectingEquipment)
	    {
		if (key == '\r')
		{
		    state = State.Overview;
		} else
		{
		    if (chooseMap.ContainsKey(key))
		    {
			inventory.Equip(chooseMap[key]);	    
			state = State.Overview;
		    } else 
		    {
			bottomMessage = "There is not such item";
		    }
		}
	    } else if (state == State.SelectingEquipped)
	    {
		if (key == '\r')
		{
		    state = State.Overview;
		} else 
		{
		    if (chooseMap.ContainsKey(key))
		    {
			inventory.UnEquip(chooseMap[key].equipmentType);
			state = State.Overview;
		    } else
		    {
			bottomMessage = "There is not such item";
		    }
		}
	    } else if (state == State.SelectingUsables)
	    {
		if (key == '\r')
		{
		    state = State.Overview;
		} else 
		{
		    if (chooseMap.ContainsKey(key))
		    {
			inventory.Use(chooseMap[key]);
			state = State.Overview;
		    } else
		    {
			bottomMessage = "There is not such item";
		    }
		}
	    }
	}

	Dictionary<char, Item> GetChooseMap(List<Item> items)
	{
	    if (items.Count == 0)
		return null;
	    Dictionary<char, Item> result = new Dictionary<char, Item>();
	    for (int i = 0; i < items.Count; i++)
	    {
		result[(char) (i + 97)] = items[i];
	    }
	    return result;
	}

	public List<string> DrawOnSide()
	{
	    var result = new List<string>();
	    switch (state)
	    {
		case State.Overview:
		    foreach(var item in inventory.GetEquipped())
		    {
			result.Add($"You have {item.name} equipped");
		    }
		    foreach(var item in inventory.items.Keys)
		    {
			result.Add(item);
		    }
		    result.Add($"");
		    result.Add($"Your health is - {inventory.player.health}");
		    return result;
		case State.SelectingEquipment:
		case State.SelectingEquipped:
		case State.SelectingUsables:
		    foreach(var item in chooseMap)
		    {
			result.Add($"{item.Key} - {item.Value.name}");
		    }
		    return result;
		default:
		    return null;
	    }
	}

	public List<string> DrawOnBottom()
	{
	    var result = new List<string>();
	    result.Add(bottomMessage);
	    return result;
	}
    }
}
