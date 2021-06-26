using System.Numerics;
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
	public List<Item> items;
	public IDrawBehaviour drawBehaviour  { get; set; }	

	public Tile(Vector2 v, bool w, char g)
	{
	    items = new List<Item>();
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
		return items[0].Draw();
	    else 
		return drawBehaviour.Draw('*');
	    } else
	    {
		return ' ';
	    }
	}

	public void PutItem(Item item)
	{
	    items.Add(item);	
	}

	public void RemoveItem(Item item)
	{
	    items.Remove(item);
	}
    }
}

