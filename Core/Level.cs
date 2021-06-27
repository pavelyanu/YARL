using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Serilog;
using YARL.Topography;
using YARL.Actors;
using YARL.Items;
using YARL.Drawing;

namespace YARL.Core
{
    public class Level
    {
	public int Width { get; protected set; }
	public int Height { get; protected set; }
	public int level { get; protected set; }
	public List<Rectangle> Rooms { get => map.Rooms; }
	Map map;
	MapGenerator mapGenerator;
	EntityFactory entityFactory;	
	List<Entity> entities;
	Dictionary<Rectangle, List<Entity>> roomPopulation;
	Player player;
	public Rectangle currentRoom { get => map.GetRoom(player.position); }
	public bool playerInCorridor { get => currentRoom.IsEmpty; }

	public Level(int w, int h, int _maxRooms, int _roomMaxSize, int _roomMinSize, int _level)
	{
	    Width = w;
	    Height = h;
	    level = _level;
	    roomPopulation = new Dictionary<Rectangle, List<Entity>>();
	    mapGenerator = new MapGenerator(Width, Height, _maxRooms, _roomMaxSize, _roomMinSize);
	    entities = new List<Entity>();
	    entityFactory = new EntityFactory(new DefaultDraw());
	    map = mapGenerator.CreateMap();
	    PopulateLevel();
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

	public void UpdateView()
	{
	    if (!playerInCorridor)
	    {
		var tiles = map.GetTilesInsideRoom(currentRoom);
		foreach (var tile in tiles)
		{
		    tile.visible = true;
		}
	    } else 
	    {
		var vectors = new List<Vector2>();
		vectors.Add(new Vector2(-1, -1));
		vectors.Add(new Vector2(0, -1));
		vectors.Add(new Vector2(1, -1));
		vectors.Add(new Vector2(1, 0));
		vectors.Add(new Vector2(1, 1));
		vectors.Add(new Vector2(0, 1));
		vectors.Add(new Vector2(-1, 1));
		vectors.Add(new Vector2(-1, 0));
		foreach (var vector in vectors)
		{
		    this[player.position + vector].visible = true;
		}
	    }
	}

	public void PopulateLevel()
	{
	    foreach(var room in Rooms)
	    {
		if (Roller.Roll(10) > 7)
		{
		    var goblin = entityFactory.CreateGoblin();
		    goblin.position = room.Center + new Vector2(Roller.Roll(2, 0), 0);
		    AddEntity(goblin);
		}

		if (Roller.Roll(10) > 7)
		{
		    var goblin = entityFactory.CreateGoblin();
		    goblin.position = room.Center + new Vector2(0, Roller.Roll(2, 0));
		    AddEntity(goblin);
		}
	    }
	}
	
	public void AddPlayer(Player p)
	{
	    player = p;
	    var room = map.GetRoom(p.position);
	    if (!room.IsEmpty)
		AddToRoom(room, player);
	}

	public Vector2 GetPlayerPosition()
	{
	    return player.position;
	}

	public bool Move(Player player, Vector2 dir)
	{
	    var result = player.position + dir;
	    if (map[result].walkable)
	    {
		if (!playerInCorridor)
		{
		    roomPopulation[currentRoom].Remove(player);
		}

		player.Move(dir);

		if (!playerInCorridor)
		{
		    AddToRoom(currentRoom, player);
		}

		return true;
	    }
	    return false;
	}

	public bool PlayerInRoomWithMonster()
	{
	    if (!playerInCorridor)
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

	public void PutItem(Item item, Vector2 position)
	{
	    map[position].items.Add(item);
	}

	public List<Tile> GetLine(Vector2 origin, Vector2 destination)
	{
	    return map.GetLine(
		(int) origin.X, (int) origin.Y, (int) destination.X, (int) destination.Y
	    ).ToList();
	}

    }
}
