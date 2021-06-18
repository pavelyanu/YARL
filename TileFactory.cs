using System.Numerics;
using System.Collections.Generic;

namespace YARL
{
    class TileFactory
	{

	    Dictionary<string, char> glyphs;

	    public TileFactory()
	    {
		glyphs = new Dictionary<string, char>();
		glyphs["floor"] = '.';
		glyphs["wall"] = ' ';
		glyphs["door"] = '+';
	    }

	    public Tile Tile(Vector2 v, bool w, char g)
	    {
		    return new Tile(v, w, g);
	    }

	    public Tile Wall(Vector2 v)
	    {
		return Tile(v, false, glyphs["wall"]);
	    }

	    public Tile Wall(int x, int y)
	    {
		return Tile(new Vector2(x, y), false, glyphs["wall"]);
	    }

	    public Tile Floor(Vector2 v)
	    {
		return Tile(v, true, glyphs["floor"]);
	    }

	    public Tile Floor(int x, int y)
	    {
		return Tile(new Vector2(x, y), true, glyphs["floor"]); 
	    }

	    public Tile Door(Vector2 v)
	    {
		return Tile(v, true, glyphs["door"]);
	    }

	    public Tile Door(int x, int y)
	    {
		return Tile(new Vector2(x, y), true, glyphs["door"]); 
	    }
	}
}
