using System.Collections.Generic;
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
	PlayerInventory inventory;
	Dictionary<char, Item> chooseMap;
	State state;	
	public bool selecting { get => 
				state == State.SelectingEquipment ||
				state == State.SelectingEquipped ||
				state == State.SelectingUsables; }
	GameLog gameLog;

	public InventoryManager(Player p, GameLog _gameLog)
	{
	    inventory = (PlayerInventory) p.inventory;
	    state = State.Overview;
	    chooseMap = null;
	    gameLog = _gameLog;
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
			gameLog.Add(inventory.Equip(chooseMap[key])); 
			state = State.Overview;
		    } else 
		    {
			gameLog.Add("There is not such item");
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
			gameLog.Add(inventory.UnEquip(chooseMap[key].equipmentType));
			state = State.Overview;
		    } else
		    {
			gameLog.Add("There is not such item");
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
			gameLog.Add(inventory.Use(chooseMap[key]));
			state = State.Overview;
		    } else
		    {
			gameLog.Add("There is not such item");
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
		result[(char) (i + 'a')] = items[i];
	    }
	    return result;
	}

	public List<string> DrawOnSide()
	{
	    var result = new List<string>();
	    switch (state)
	    {
		case State.Overview:
		    foreach(Item item in inventory.GetEquipped())
		    {
			result.Add($"You have {item.name} equipped");
		    }
		    foreach(string item in inventory.items.Keys)
		    {
			if (inventory.items[item].amount == 1)
			{
			    result.Add(item);
			} else 
			{
			    result.Add($"{item} ({inventory.items[item].amount})");
			}
		    }
		    return result;
		case State.SelectingEquipment:
		case State.SelectingEquipped:
		case State.SelectingUsables:
		    result.Add("Select an item");
		    foreach(char key in chooseMap.Keys)
		    {
			Item item = chooseMap[key];
			result.Add($"{key} - {item.name}");
		    }
		    return result;
		default:
		    return null;
	    }
	}
    }
}
