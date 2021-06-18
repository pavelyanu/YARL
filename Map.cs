using System;
using System.Numerics;
using System.Collections.Generic;

namespace YARL
{
    public class Map
    {
	Dictionary<Vector2, Tile> tiles;
	public List<Rectangle> Rooms;
	public int Height { get; set; }
	public int Width { get; set; }

	public Map(int h, int w)
	{
	    Height = h;
	    Width = w;
	    tiles = new Dictionary<Vector2, Tile>();
	    Rooms = new List<Rectangle>();
	}

	public Tile this[Vector2 key]
	{
	    get { 
		if (tiles.ContainsKey(key))
		return tiles[key];
		else 
		return null;
	    }
	    set {
		tiles[key] = value;
	    }
	}
	
	public Tile this[int x, int y]
	{
	    get { 
		Vector2 key = new Vector2(x, y);
		if (tiles.ContainsKey(key))
		return tiles[key];
		else 
		return null;
	    }

	    set {
		Vector2 key = new Vector2(x, y);
		tiles[key] = value;
	    }
	}
	
	public void SetCell(Tile tile)
	{
	    tiles[tile.position] = tile;
	}

	public IEnumerable<Tile> GetLine(
	    int xOrigin, int yOrigin, int xDestination, int yDestination
	)
	{
	    xOrigin = ClampX( xOrigin );
	    yOrigin = ClampY( yOrigin );
	    xDestination = ClampX( xDestination );
	    yDestination = ClampY( yDestination );

	    int dx = Math.Abs( xDestination - xOrigin );
	    int dy = Math.Abs( yDestination - yOrigin );

	    int sx = xOrigin < xDestination ? 1 : -1;
	    int sy = yOrigin < yDestination ? 1 : -1;
	    int err = dx - dy;
	    while ( true )
	    {
		if (tiles.ContainsKey(new Vector2(xOrigin, yOrigin)))
		{
		   yield return this[ xOrigin, yOrigin ];
		} else
		{
		    break;
		}
		
		if ( xOrigin == xDestination && yOrigin == yDestination )
		{
		    break;
		}
		int e2 = 2 * err;
		if ( e2 > -dy )
		{
		    err = err - dy;
		    xOrigin = xOrigin + sx;
		}
		if ( e2 < dx )
		{
		    err = err + dx;
		    yOrigin = yOrigin + sy;
		}
	    }
	}

      private int ClampX( int x )
      {
         return ( x < 0 ) ? 0 : ( x > Width - 1 ) ? Width - 1 : x;
      }

      private int ClampY( int y )
      {
         return ( y < 0 ) ? 0 : ( y > Height - 1 ) ? Height - 1 : y;
      }

    }
}
