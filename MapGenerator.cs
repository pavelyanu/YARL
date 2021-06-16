using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace YARL
{
   public class MapGenerator
   {
      private readonly int _width;
      private readonly int _height;
      private readonly int _maxRooms;
      private readonly int _roomMaxSize;
      private readonly int _roomMinSize;
      private readonly int _level;
      private readonly Map _map;
      private Random random;

      public MapGenerator( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, int level )
      {
         _width = width;
         _height = height;
         _maxRooms = maxRooms;
         _roomMaxSize = roomMaxSize;
         _roomMinSize = roomMinSize;
         _level = level;
         _map = new Map(width, height);
	 random = new Random();
      }

      public Map CreateMap()
      {
         for ( int r = 0; r < _maxRooms; r++ )
         {
            int roomWidth = random.Next( _roomMinSize, _roomMaxSize );
            int roomHeight = random.Next( _roomMinSize, _roomMaxSize );
            int roomXPosition = random.Next( 0, _width - roomWidth - 1 );
            int roomYPosition = random.Next( 0, _height - roomHeight - 1 );

            var newRoom = new Rectangle( roomXPosition, roomYPosition, roomWidth, roomHeight );
            bool newRoomIntersects = _map.Rooms.Any( room => newRoom.Intersects( room ) );
	    bool onEdge = newRoom.Left == _width
			|| newRoom.Right == 0
			|| newRoom.Top == 0
			|| newRoom.Bottom == _height;

            if ( !newRoomIntersects )
            {
               _map.Rooms.Add( newRoom );
            }
         }

         foreach ( Rectangle room in _map.Rooms )
         {
            CreateMap( room );
         }

         for ( int r = 0; r < _map.Rooms.Count; r++ )
         {
            if ( r == 0 )
            {
               continue;
            }

            int previousRoomCenterX = (int) _map.Rooms[r - 1].Center.X;
            int previousRoomCenterY = (int) _map.Rooms[r - 1].Center.Y;
            int currentRoomCenterX = (int) _map.Rooms[r].Center.X;
            int currentRoomCenterY = (int) _map.Rooms[r].Center.Y;

            if ( random.Next( 0, 2 ) == 0 )
            {
               CreateHorizontalTunnel( previousRoomCenterX, currentRoomCenterX, previousRoomCenterY );
               CreateVerticalTunnel( previousRoomCenterY, currentRoomCenterY, currentRoomCenterX );
            }
            else
            {
               CreateVerticalTunnel( previousRoomCenterY, currentRoomCenterY, previousRoomCenterX );
               CreateHorizontalTunnel( previousRoomCenterX, currentRoomCenterX, currentRoomCenterY );
            }
         }

	 AddWalls(_map);

         foreach ( Rectangle room in _map.Rooms )
         {
            CreateDoors( room );
         }

         return _map;
      }

      private void CreateMap( Rectangle room )
      {
         for ( int x = room.Left + 1; x < room.Right; x++ )
         {
            for ( int y = room.Top + 1; y < room.Bottom; y++ )
            {
		
		Floor floor = new Floor(new Vector2(x, y));
               _map.SetCell(floor);
            }
         }
      }

      private void CreateHorizontalTunnel( int xStart, int xEnd, int yPosition )
      {
         for ( int x = Math.Min( xStart, xEnd ); x <= Math.Max( xStart, xEnd ); x++ )
         {
		Floor floor = new Floor(new Vector2(x, yPosition));
               _map.SetCell(floor);
         }
      }

      private void CreateVerticalTunnel( int yStart, int yEnd, int xPosition )
      {
         for ( int y = Math.Min( yStart, yEnd ); y <= Math.Max( yStart, yEnd ); y++ )
         {	
		Floor floor = new Floor(new Vector2(xPosition, y));
               _map.SetCell(floor);
         }
      }

      private void CreateDoors( Rectangle room )
      {
         int xMin = room.Left;
         int xMax = room.Right;
         int yMin = room.Top;
         int yMax = room.Bottom;

         List<Tile> borderTiles = _map.GetLine( xMin, yMin, xMax, yMin ).ToList();
         borderTiles.AddRange( _map.GetLine( xMin, yMin, xMin, yMax ) );
         borderTiles.AddRange( _map.GetLine( xMin, yMax, xMax, yMax ) );
         borderTiles.AddRange( _map.GetLine( xMax, yMin, xMax, yMax ) );

         foreach ( Tile cell in borderTiles )
         {
            if ( IsPotentialDoor( cell ) )
            {
		Door door = new Door(new Vector2(cell.position.X, cell.position.Y));
		_map.SetCell(door);
            }
         }
      }

      private bool IsPotentialDoor( Tile cell )
      {
         if ( !cell.walkable )
         {
            return false;
         }

         Tile right = _map[(int) cell.position.X + 1, (int) cell.position.Y];
         Tile left = _map[(int) cell.position.X - 1, (int) cell.position.Y];
         Tile top = _map[(int) cell.position.X, (int) cell.position.Y - 1];
         Tile bottom = _map[(int) cell.position.X, (int) cell.position.Y + 1];

         if ( _map[(int) cell.position.X, (int) cell.position.Y ] is Door ||
              _map[(int) right.position.X, (int) right.position.Y ] is Door ||
              _map[(int) left.position.X, (int) left.position.Y ] is Door ||
              _map[(int) top.position.X, (int) top.position.Y ] is Door ||
              _map[(int) bottom.position.X, (int) bottom.position.Y ] is Door )
         {
            return false;
         }

         if ( right.walkable && left.walkable && !top.walkable && !bottom.walkable )
         {
            return true;
         }
         if ( !right.walkable && !left.walkable && top.walkable && bottom.walkable )
         {
            return true;
         }
         return false;
      }

	private void AddWalls(Map map)
	{
	    for (int x = 0; x < _width; x++)
	    {
		for (int y = 0; y < _height; y++) {
		    if (map[x, y] is null)
		    {
			Wall wall = new Wall(x, y);
			map.SetCell(wall);
		    }
		}
	    }
	}
    }
}
