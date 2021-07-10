using System;
using System.Collections.Generic;
using YARL.Items;

namespace YARL.Actors
{
    public class PlayerInventory : Inventory
    {
        Dictionary<EquipmentType, Item> equipment;

        public Player player;

        public PlayerInventory(Player p)
        {
            items = new Dictionary<string, Item>();
            equipment = new Dictionary<EquipmentType, Item>();
            player = p;
        }

        public string Equip(Item item)
        {
            if (!items.ContainsKey(item.name))
                throw new ArgumentException($"{item.name} is not in the inventory");
	    if (!item.equipable)
                throw new ArgumentException($"{item.name} is not equipable");
            if (!equipment.ContainsKey(item.equipmentType) || equipment[item.equipmentType] is null)
            {
		if (item.CanEquip(player))
		{
		    equipment[item.equipmentType] = item;
		    Remove(item);
		}
                return item.Equip(player);
            } else
	    {
		if (item.CanEquip(player))
		{
		    UnEquip(item.equipmentType);
		    equipment[item.equipmentType] = item;
		    Remove(item);
		}
		return item.Equip(player);
	    }
        }

        public string UnEquip(EquipmentType equipmentType)
        {
            if (!equipment.ContainsKey( equipmentType ) || equipment[equipmentType] is null)
                throw new ArgumentException($"Nothing is equipped in this slot.");

            Item item = equipment[equipmentType];
            equipment.Remove(equipmentType);
	    item.amount++;
            Add(item);
            return item.UnEquip(player);
        }

	public string Use(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		Remove(item);
		return item.Use(player);
	    } else 
	    {
		throw new ArgumentException($"there is on {item.name} in the inventory"); 
	    }
	}

        public List<Item> GetEquipable()
        {
            var result = new List<Item>();
            foreach (var item in items.Values)
            {
                if (item.equipable)
                    result.Add(item);
            }
            return result;
        }

	public List<Item> GetEquipped()
	{
	    var result = new List<Item>();
            foreach (var item in equipment.Values)
            {
                result.Add(item);
            }
            return result;
	}

        public List<Item> GetUsable()
        {
            var result = new List<Item>();
            foreach (var item in items.Values)
            {
                if (item.usable)
                    result.Add(item);
            }
            return result;
        }

        public List<Item> GetAll(PossessionType possessionType)
        {
            var result = new List<Item>();
            foreach (var item in items.Values)
            {
                if (item.possessionType == possessionType)
                    result.Add(item);
            }
            return result;
        }

        public List<Item> GetAll(EquipmentType equipmentType)
        {
            var result = new List<Item>();
            foreach (var item in items.Values)
            {
                if (item.equipmentType == equipmentType)
                    result.Add(item);
            }
            return result;
        }
    }
}
