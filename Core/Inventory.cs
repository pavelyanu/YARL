using System;
using System.Collections.Generic;
using YARL.Items;
using YARL.Actors;

namespace YARL.Core
{
    public class Inventory
    {
        public Dictionary<string, Item> items;	

        Dictionary<EquipmentType, Item> equipment;

        Player player;

        public Inventory(Player p)
        {
            items = new Dictionary<string, Item>();
            equipment = new Dictionary<EquipmentType, Item>();
            player = p;
        }

        public void EquipItem(Item item)
        {
            if (!items.ContainsKey(item.name))
                throw new ArgumentException($"{item.name} is not in the inventory");
            if (!equipment.ContainsKey(item.equipmentType) || equipment[item.equipmentType] is null)
            {
                if (!item.equipable)
                    throw new ArgumentException($"{item.name} is not equipable");
                equipment[item.equipmentType] = item;
                if (items[item.name].amount-- == 0)
                    Remove(item);
                item.Equip(player);
            }
        }

        public void UnEquip(EquipmentType equipmentType)
        {
            if (!equipment.ContainsKey( equipmentType ) || equipment[equipmentType] is null)
                throw new ArgumentException($"Nothing is equipped in this slot.");

            Item item = equipment[equipmentType];
            equipment.Remove(equipmentType);
            item.UnEquip(player);
            Add(item);
        }

	public void Add(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		items[item.name].amount++;
	    }else
	    {
		items[item.name] = item;
	    }
	}

	public void Remove(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		items[item.name].amount--;
		if (items[item.name].amount == 0)
		{
		    items.Remove(item.name);
		}
	    } else 
	    {
		throw new ArgumentException($"there is on {item.name} in the inventory"); 
	    }
	}


        public Item Hands {
            get => Hands;
            set
            {
                if (Hands is null) Hands = value;
            }
        }

        public Item Body {
            get => Body;
            set
            {
                if (Body is null) Body = value;
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
