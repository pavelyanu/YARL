using System.Numerics;
using System.Collections.Generic;
using YARL.Drawing;

namespace YARL.Topography
{
    class TileFactory
	{

	    Dictionary<string, char> glyphs;
	    IDrawBehaviour drawBehaviour;

	    public TileFactory()
	    {
		glyphs = new Dictionary<string, char>();
		glyphs["floor"] = '.';
		glyphs["wall"] = '#';
		glyphs["door"] = '+';
		drawBehaviour = new DefaultDraw();
	    }

	    public Tile Tile(Vector2 v, bool w, char g)
	    {
		    var tile = new Tile(v, w, g);
		    tile.drawBehaviour = drawBehaviour;
		    return tile;
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

	    public Tile Empty(Vector2 v)
	    {
		return Tile(v, false, ' ');
	    }

	    public Tile Empty(int x, int y)
	    {
		return Tile(new Vector2(x, y), false, ' ');
	    }
	}
}
