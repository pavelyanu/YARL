using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using YARL.Drawing;
using YARL.Items;

namespace YARL.Topography
{
    public class Tile: IDrawable
    {
	public Vector2 position { get; protected set; } 
	public bool walkable { get; protected set; }
	public bool visible { get; set; }
	public char glyph { get; protected set; }
	public Dictionary<string, Item> items;
	public IDrawBehaviour drawBehaviour  { get; set; }	

	public Tile(Vector2 v, bool w, char g)
	{
	    items = new Dictionary<string, Item>();
	    position = v;
	    walkable = w;
	    glyph = g;
	    visible = false;
	}

	public char Draw()
	{
	    if (visible)
	    {
	    if (items.Count == 0)
		return drawBehaviour.Draw(glyph);
	    else if(items.Count == 1)
		return items.Values.ToList()[0].Draw();
	    else 
		return drawBehaviour.Draw('*');
	    } else
	    {
		return ' ';
	    }
	}

	public void PutItem(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		items[item.name].amount += item.amount;
	    } else 
	    {
		items[item.name] = item;
	    }
	}

	public void RemoveItem(Item item)
	{
	    if (items.ContainsKey(item.name))
	    {
		items.Remove(item.name);
	    }
	}
    }
}

