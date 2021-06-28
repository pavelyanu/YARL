using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
	ItemFactory itemFactory;
	List<Entity> entities;
	Dictionary<Rectangle, List<Entity>> roomPopulation;
	Dictionary<char, Item> chooseMap;
	Player player;
	string bottomMessage;
	public Rectangle currentRoom { get => map.GetRoom(player.position); }
	public bool playerInCorridor { get => currentRoom.IsEmpty; }
	public bool standingOnItems { get => !(this[player.position].items is null ||
					    this[player.position].items.Count == 0); }
	public bool choosingItem;
	public bool playerHasWon { get => entities.Count == 0 && player.inventory.items.ContainsKey("Gem"); }

	public Level(int w, int h, int _maxRooms, int _roomMaxSize, int _roomMinSize, int _level)
	{
	    Width = w;
	    Height = h;
	    level = _level;
	    choosingItem = false;
	    bottomMessage = "";
	    roomPopulation = new Dictionary<Rectangle, List<Entity>>();
	    chooseMap = new Dictionary<char, Item>();
	    mapGenerator = new MapGenerator(Width, Height, _maxRooms, _roomMaxSize, _roomMinSize);
	    entities = new List<Entity>();
	    entityFactory = new EntityFactory(new DefaultDraw());
	    itemFactory = new ItemFactory(new DefaultDraw());
	    map = mapGenerator.CreateMap();
	    Prepare();
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

	public bool ProcessInput(char key)
	{
	    if (entities.Count == 0)
	    {
		bottomMessage = "You have killed all the monsters. Now you have to find a gem";
	    }
	    bool result = false;
	    if (choosingItem)
	    {
		if (key == '\r')
		{
		    choosingItem = false;
		}
		if (chooseMap.ContainsKey(key))
		{
		    PlayerPickItem(chooseMap[key].name);
		    if (chooseMap[key].name == "Gem")
			bottomMessage = "You have found the gem. Now you have to kill all the monsters!";
		    choosingItem = false;
		} else 
		{
		    bottomMessage = "There is no such item there";
		}
		return result;
	    }
	    if ("hjkl".Contains(key))
	    {
		bottomMessage = "";
	    }
	    if (key == 'k')
		result = Move(player, new Vector2(0, -1));
	    else if (key == 'h')
		result = Move(player, new Vector2(-1, 0));
	    else if (key == 'j')
		result = Move(player, new Vector2(0, 1));
	    else if (key == 'l')
		result = Move(player, new Vector2(1, 0));
	    else if (key ==',')
	    {
		if (standingOnItems)
		{
		    chooseMap = GetChooseMap(this[player.position].items);
		    choosingItem = true;
		}
	    }
	    return result;
	}

	Dictionary<char, Item> GetChooseMap(List<Item> items)
	{
	    if (items.Count == 0)
		return null;
	    Dictionary<char, Item> result = new Dictionary<char, Item>();
	    for (int i = 0; i < items.Count; i++)
	    {
		result[(char) (i + 97)] = items[i];
	    }
	    return result;
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

	public void Prepare()
	{
	    var firstRoom = Rooms[0];
	    PutItem(itemFactory.CreateDagger(), firstRoom.Center + new Vector2(0, 1));
	    PutItem(itemFactory.CreateHealingPotion(), firstRoom.Center + new Vector2(1, 1));
	    var lastRoom = Rooms[Rooms.Count - 1];
	    PutItem(itemFactory.CreateGoalGem(), lastRoom.Center);
	    var ork1 = entityFactory.CreateOrk();
	    ork1.position = lastRoom.Center + new Vector2(Roller.Roll(3), 2);
	    AddEntity(ork1);
	    var bowmen = entityFactory.CreateGoblinWithBow();
	    bowmen.position = lastRoom.Center + new Vector2(Roller.Roll(3), 1);
	    AddEntity(bowmen);
	}

	public void PopulateLevel()
	{
	    foreach(var room in Rooms)
	    {
		if (Rooms[0] == room || Rooms[Rooms.Count - 1] == room)
		    continue;
		int number = Roller.Roll(3) - 1;
		for (int i = 0; i < number; i++)
		{
		    int type = Roller.Roll(5);
		    switch (type)
		    {
			case 1:
			case 2:
			    var goblin = entityFactory.CreateGoblin();
			    goblin.position = room.Center + new Vector2(Roller.Roll(3), i);
			    AddEntity(goblin);
			    break;
			case 3:
			case 4:
			    var bowmen = entityFactory.CreateGoblinWithBow();
			    bowmen.position = room.Center + new Vector2(Roller.Roll(3), i);
			    AddEntity(bowmen);
			    break;
			case 5:
			    var ork = entityFactory.CreateOrk();
			    ork.position = room.Center + new Vector2(Roller.Roll(3), i);
			    AddEntity(ork);
			    break;
		    }
		}
		if (Roller.Roll(10) > 7)
		    PutItem(itemFactory.CreateHealingPotion(), room.Center + new Vector2(Roller.Roll(2), 2));
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
		    return true;
		}
	    }
	    return false;
	}

	public void PlayerPickItem(string name)
	{
	    var position = player.position;
	    if (map[position].items.Count != 0)
	    {
		int index = -1;
		foreach(var item in map[position].items)
		{
		    if (item.name == name)
		    {
			item.Pick(player);
			index = map[position].items.IndexOf(item);
		    }
		}
		if (index != -1)
		    map[position].items.RemoveAt(index);
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
	    bool found = false;
	    foreach(var i in map[position].items)
	    {
		if (i.name == item.name)
		    i.amount++;
	    }
	    if (!found)
		map[position].items.Add(item);
	}

	public List<Tile> GetLine(Vector2 origin, Vector2 destination)
	{
	    return map.GetLine(
		(int) origin.X, (int) origin.Y, (int) destination.X, (int) destination.Y
	    ).ToList();
	}

	public int GetDistance(Vector2 origin, Vector2 destination)
	{
	    return GetLine(origin, destination).Count;
	}
	
	public List<string> DrawOnSide()
	{
	    var result = new List<string>();
	    foreach(var item in chooseMap)
	    {
		result.Add($"{item.Key} - {item.Value.name}");
	    }
	    return result;
	}

	public List<string> DrawOnBottom()
	{
	    var result = new List<string>();
	    result.Add(bottomMessage);
	    return result;
	}
    }
}
