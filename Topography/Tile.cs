using System.Numerics;
using YARL.Drawing;

namespace YARL.Topography
{
    public class Tile: Drawable
    {
	public Vector2 position { get; protected set; } 
	public bool walkable { get; protected set; }

	public Tile(Vector2 v, bool w, char g)
	{
	    position = v;
	    walkable = w;
	    glyph = g;
	}
    }
}

