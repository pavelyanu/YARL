using System.Numerics;

namespace YARL
{
    public class Tile: IDrawable
    {
	public Vector2 position { get; protected set; } 
	public bool walkable { get; protected set; }
	public char glyph {get; protected set; }

	public Tile(Vector2 v, bool w, char g)
	{
	    position = v;
	    walkable = w;
	    glyph = g;
	}

	public char Draw()
	{
	    return glyph;
	}
    }
}

