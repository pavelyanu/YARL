using System;
using Serilog;
using System.Numerics;
using System.Collections.Generic;

namespace YARL.Topography
{
    public class Map
    {
	Tile[,] tiles;
	public List<Rectangle> Rooms;
	public int Height { get; set; }
	public int Width { get; set; }

	public Map(int w, int h)
	{
	    Height = h;
	    Width = w;
	    tiles = new Tile[w, h];
	    Rooms = new List<Rectangle>();
	}

        public Tile this[Vector2 key] {
            get
	    {
		if (key.X < Width && key.X >= 0 && key.Y < Height && key.Y >= 0)
		{
		    return tiles[(int) key.X, (int) key.Y];
		}
		return null;
	    }
	    set
	    {
		if (key.X < Width && key.X >= 0 && key.Y < Height && key.Y >= 0)
		{
		    tiles[(int) key.X, (int) key.Y] = value;
		} 
	    }
        }

	public Tile this[int x, int y] {
            get
	    {
		if (x < Width && x >= 0 && y < Height && y >= 0)
		{
		    return tiles[x, y];
		}
		return null;
	    }
	    set
	    {
		if (x < Width && x >= 0 && y < Height && y >= 0)
		{
		    tiles[x, y] = value;
		}
	    }
        }

	public void SetCell(Tile tile)
	{
	    this[tile.position] = tile;
	}

        public bool isDoor(Vector2 v) =>
            this[v] is not null && this[v].glyph == '+';

	public bool IsDoor(int x, int y)
	{
	    var v = new Vector2(x, y);
	    return isDoor(v);
	}

	public IEnumerable<Tile> GetLine(
		int xOrigin, int yOrigin, int xDestination, int yDestination
	)
	{
	    Log.Information("Running GetLine");
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
		if (this[new Vector2(xOrigin, yOrigin)] is not null)
		{
		    yield return this[ xOrigin, yOrigin ];
		} else
		{
		    continue;
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

	public Rectangle GetRoom( Vector2 position)
	{
	    foreach(Rectangle room in Rooms)
	    {
		if (room.Contains(position))
		{
		    return room;
		}
	    }
	    
	    return new Rectangle();
	}

	public List<Tile> GetTilesInsideRoom(Rectangle room)
	{
	    var result = new List<Tile>();
	    if (Rooms.Contains(room))
	    {
		for (int x = room.Left; x <= room.Right; x++)
		{
		    for (int y = room.Top; y <= room.Bottom; y++)
		    {
			result.Add(this[x, y]);
		    }
		}
	    }
	    return result;
	}

    }
}
