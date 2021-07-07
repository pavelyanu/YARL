using System;
using System.Collections.Generic;
using YARL.Items;

namespace YARL.Actors
{
    public abstract class Inventory
    {
        public Dictionary<string, Item> items;	

	public Inventory()
	{
	    items = new Dictionary<string, Item>();
	}

	public void Add(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		items[item.name].amount += item.amount;
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
		if (items[item.name].amount <= 0)
		{
		    items.Remove(item.name);
		}
	    } else 
	    {
		throw new ArgumentException($"there is on {item.name} in the inventory"); 
	    }
	}
    }
}
