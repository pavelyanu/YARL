using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Serilog;
using YARL.Topography;
using YARL.Actors;
using YARL.Items;

namespace YARL.Core
{
    class Level
    {
	public int Width { get; protected set; }
	public int Height { get; protected set; }
	public int maxRooms { get; protected set; }
	public int roomMaxSize { get; protected set; }
	public int roomMinSize { get; protected set; }
	public int level { get; protected set; }
	public List<Rectangle> Rooms { get => map.Rooms; }
	Map map;
	MapGenerator mapGenerator;
	List<Entity> entities;
	Dictionary<Rectangle, List<Entity>> roomPopulation;
	Player player;
	public Rectangle currentRoom { get => map.GetRoom(player.position); }
	public bool PlayerInCorridor { get => currentRoom.IsEmpty; }

	public Level(int w, int h, int _maxRooms, int _roomMaxSize, int _roomMinSize, int _level)
	{
	    Width = w;
	    Height = h;
	    maxRooms = _maxRooms;
	    roomMaxSize = _roomMaxSize;
	    roomMinSize = _roomMinSize;
	    level = _level;
	    roomPopulation = new Dictionary<Rectangle, List<Entity>>();
	    mapGenerator = new MapGenerator(Width, Height, maxRooms, roomMaxSize, roomMinSize);
	    entities = new List<Entity>();
	    map = mapGenerator.CreateMap();
	}

	public Tile this[Vector2 v]
	{
	    get
	    {
		return map[v];
	    }
	}

	public Tile this[int x, int y]
	{
	    get
	    {
		return map[x, y];
	    }
	}
	
	public void AddPlayer(Player p)
	{
	    player = p;
	    var room = map.GetRoom(p.position);
	    if (!room.IsEmpty)
		AddToRoom(room, player);
	}

	public bool Move(Player player, Vector2 dir)
	{
	    var result = player.position + dir;
	    if (map[result].walkable)
	    {
		if (!PlayerInCorridor)
		{
		    roomPopulation[currentRoom].Remove(player);
		}

		player.Move(dir);

		if (!PlayerInCorridor)
		{
		    AddToRoom(currentRoom, player);
		}

		return true;
	    }
	    return false;
	}

	public bool PlayerInRoomWithMonster()
	{
	    if (!PlayerInCorridor)
	    {
		if (roomPopulation[currentRoom].Count > 1)
		{
		    Log.Information("Player is in the room with: ");
		    Log.Information(roomPopulation[currentRoom][0].ToString());
		    Log.Information(roomPopulation[currentRoom][1].ToString());
		    return true;
		}
	    }
	    return false;
	}

	public void PlayerPickItem()
	{
	    var position = player.position;
	    if (map[position].items.Count != 0)
	    {
		var item = map[position].items[0];
		item.Pick(player);
		map[position].items.Remove(item);
	    }
	}

	public void AddEntity(Entity entity)
	{
	    var room = map.GetRoom(entity.position);
	    if (!room.IsEmpty)
	    {
		entities.Add(entity);
		AddToRoom(room, entity);	
	    }
	}

	public void RemoveEntity(Entity entity)
	{
	    entities.Remove(entity);
	    roomPopulation[map.GetRoom(entity.position)].Remove(entity);
	}


	public bool Move(Entity entity, Vector2 dir)
	{
	    var result = entity.position + dir;
	    var room = map.GetRoom(entity.position);
	    if (map[result].walkable && !room.IsEmpty)
	    {
		entity.Move(dir);
		return true;
	    }
	    return false;
	}

	void RemoveFromRoom(Rectangle room, Entity entity)
	{
	    roomPopulation[room].Remove(entity);
	    if (roomPopulation[room].Count == 0)
		roomPopulation.Remove(room);
	}

	void AddToRoom(Rectangle room, Entity entity)
	{
	    if (!roomPopulation.ContainsKey(room))
		roomPopulation[room] = new List<Entity>();

	    roomPopulation[room].Add(entity);
	}

	public List<Monster> GetMonstersNearPlayer()
	{
	    var result = new List<Monster>();
	    foreach (var entity in roomPopulation[currentRoom])
	    {
		if (entity is Monster)
		    result.Add(entity as Monster);
	    }
	    return result;
	}

	public void AddItem(Pickable item)
	{
	    map[item.position].items.Add(item);
	}

	public List<Tile> GetLine(Vector2 origin, Vector2 destination)
	{
	    return map.GetLine(
		(int) origin.X, (int) origin.Y, (int) destination.X, (int) destination.Y
	    ).ToList();
	}

    }
}
