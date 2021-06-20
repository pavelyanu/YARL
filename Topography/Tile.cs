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
	public char glyph { get; protected set; }
	public List<Pickable> items;
	public IDrawingBehaviour drawingBehaviour  { get; protected set; }	

	public Tile(Vector2 v, bool w, char g)
	{
	    items = new List<Pickable>();
	    position = v;
	    walkable = w;
	    glyph = g;
	}

	public char Draw()
	{
	    if (items.Count == 0)
		return drawingBehaviour.Draw(glyph);
	    else if(items.Count == 1)
		return items[0].Draw();
	    else 
		return drawingBehaviour.Draw('*');
	}

	public void SetDrawingBehaviour(IDrawingBehaviour b)
	{
	    drawingBehaviour = b;
	}

	public void PutItem(Pickable item)
	{
	    items.Add(item);	
	}

	public void RemoveItem(Pickable item)
	{
		items.Remove(item);
	}
    }
}

