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
	EntityFactory entityFactory;	
	ItemFactory itemFactory;
	List<Entity> entities;
	GameLog gameLog;
	Dictionary<Rectangle, List<Entity>> roomPopulation;
	Dictionary<char, Item> chooseMap;
	Player player;
	public Rectangle currentRoom { get => map.GetRoom(player.position); }
	public bool playerInCorridor { get => currentRoom.IsEmpty; }
	public bool standingOnItems { get => !(this[player.position].items is null ||
					    this[player.position].items.Count == 0); }
	public bool printedPreFinal;
	public bool choosingItem;

	public Level(
	    int w,
	    int h,
	    int _maxRooms,
	    int _roomMaxSize,
	    int _roomMinSize,
	    int _level,
	    GameLog _gameLog,
	    Player _player
	)
	{
	    Width = w;
	    Height = h;
	    level = _level;
	    gameLog = _gameLog;
	    choosingItem = false;
	    printedPreFinal = false;
	    roomPopulation = new Dictionary<Rectangle, List<Entity>>();
	    chooseMap = new Dictionary<char, Item>();
	    var mapGenerator = new MapGenerator(Width, Height, _maxRooms, _roomMaxSize, _roomMinSize);
	    entities = new List<Entity>();
	    entityFactory = new EntityFactory(new DefaultDraw());
	    itemFactory = new ItemFactory(new DefaultDraw());
	    player = _player;
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
		    choosingItem = false;
		} else 
		{
		    WriteToLog("There is no such item there");
		}
		return result;
	    } else
	    {
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
			chooseMap = GetChooseMap(this[player.position].items.Values.ToList());
			choosingItem = true;
		    }
		} else if (playerInCorridor || GetMonstersNearPlayer().Count == 0)
		{
		    Log.Information("Entering shift key switch");
		    if ("HJKL".Contains(key))
		    {
			Log.Information($"Key {key} is found");
			key = (char) ((int) key + 32);
			Log.Information($"Key was transformed to {key}");
			result = true;
			while(GetMonstersNearPlayer().Count == 0)
			{
			    if (!ProcessInput(key))
				break;
			    UpdateView();
			}
		    }
		}
		return result;
	    }
	}

	Dictionary<char, Item> GetChooseMap(List<Item> items)
	{
	    if (items.Count == 0)
		return null;
	    Dictionary<char, Item> result = new Dictionary<char, Item>();
	    for (int i = 0; i < items.Count; i++)
	    {
		result[(char) (i + 'a')] = items[i];
	    }
	    return result;
	}

	public void UpdateView()
	{
	    if (!playerInCorridor)
	    {
		List<Tile> tiles = map.GetTilesInsideRoom(currentRoom);
		foreach (var tile in tiles)
		{
		    tile.visible = true;
		}
	    } else 
	    {
		var vectors = new List<Vector2>();
		for (int x = -1 ; x <= 1 ; ++x)
                    for (int y = -1 ; y <= 1 ; ++y)
                        if ((x, y) != (0, 0))
                            vectors.Add(new Vector2(x, y));
		foreach (var vector in vectors)
		{
		    this[player.position + vector].visible = true;
		}
	    }
	}

	public void Prepare()
	{
	    Rectangle firstRoom = Rooms[0];
	    player.position = firstRoom.Center;
	    AddPlayer(player);
	    Rectangle lastRoom = Rooms[Rooms.Count - 1];
	    if (level == 1)
	    {
		PutItem(itemFactory.CreateDagger(), firstRoom.Center + new Vector2(0, 1));
		PutItem(itemFactory.CreateHealingPotion(), firstRoom.Center + new Vector2(1, 1));
		Entity ork1 = entityFactory.CreateOrc();
		Item gem = itemFactory.CreateGoalGem();
		ork1.inventory.Add(itemFactory.CreateGoalGem());
		ork1.position = lastRoom.Center + new Vector2(Roller.Roll(3), 2);
		AddEntity(ork1);
		Entity bowmen = entityFactory.CreateGoblinWithBow();
		bowmen.position = lastRoom.Center + new Vector2(Roller.Roll(3), 1);
		AddEntity(bowmen);
	    } else if (level == 2)
	    {
		Entity necromanser = entityFactory.CreateNecromancer();
		necromanser.inventory.Add(itemFactory.CreateGoalGem());
		necromanser.position = lastRoom.Center + new Vector2(Roller.Roll(3), 2);
		AddEntity(necromanser);
		Entity ghoul1 = entityFactory.CreateGhoul();
		ghoul1.position = lastRoom.Center + new Vector2(Roller.Roll(3), 1);
		AddEntity(ghoul1);
		Entity ghoul2 = entityFactory.CreateGhoul();
		ghoul2.position = lastRoom.Center + new Vector2(Roller.Roll(3), 3);
		AddEntity(ghoul2);
	    } else if (level == 3)
	    {
		Entity king = entityFactory.CreateGnomeKing();
		king.inventory.Add(itemFactory.CreateGoalGem());
		king.position = lastRoom.Center + new Vector2(Roller.Roll(3), 2);
		AddEntity(king);
		Entity archer1 = entityFactory.CreateArcherGnome();
		archer1.position = lastRoom.Center + new Vector2(Roller.Roll(3), 1);
		AddEntity(archer1);
		Entity archer2 = entityFactory.CreateArcherGnome();
		archer2.position = lastRoom.Center + new Vector2(Roller.Roll(3), 3);
		AddEntity(archer2);
	    }
	}

	public void PopulateLevel()
	{
	    foreach(var room in Rooms)
	    {
		if (Rooms[0] == room || Rooms[Rooms.Count - 1] == room)
		    continue;

                int number = Roller.Roll(level == 1 ? 3 : 4) - 1;
		for (int i = 0; i < number; i++)
		{
		    int type = Roller.Roll(6);
		    switch (type)
		    {
			case 1:
			case 2:
			case 3:
			    if (level == 1)
			    {
				Entity goblin = entityFactory.CreateGoblin();
				goblin.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(goblin);
			    } else if (level == 2) 
			    {
				int roll = Roller.Roll(5);
				Entity zombie1 = entityFactory.CreateZombie();
				Entity zombie2 = entityFactory.CreateZombie();
				Entity skeleton = entityFactory.CreateSkeleton();
				if (roll < 3)
				{
				    zombie1.position = room.Center + new Vector2(Roller.Roll(3), i);
				    AddEntity(zombie1);
				} else if (roll < 5)
				{
				    skeleton.position = room.Center + new Vector2(Roller.Roll(3), i + 1);
				    AddEntity(skeleton);
				} else 
				{
				    zombie1.position = room.Center + new Vector2(Roller.Roll(3), i);
				    AddEntity(zombie1);
				    skeleton.position = room.Center + new Vector2(Roller.Roll(3), i + 1);
				    AddEntity(skeleton);
				}
			    } else if (level == 3)
			    {
				Entity gnome = entityFactory.CreateGnome();
				gnome.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(gnome);
			    }
			    break;
			case 4:
			case 5:
			    if (level == 1)
			    {
				Entity bowmen = entityFactory.CreateGoblinWithBow();
				bowmen.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(bowmen);
			    } else if (level == 2)
			    {
				Entity ghoul = entityFactory.CreateGhoul();
				ghoul.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(ghoul);
			    } else if (level == 3)
			    {
				Entity gnome = entityFactory.CreateArcherGnome();
				gnome.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(gnome);
			    }
			    break;
			case 6:
			    if (level == 1)
			    {
				Entity goblin = entityFactory.CreateGoblin();
				goblin.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(goblin);
			    } else if (level == 2)
			    {
				Entity ork = entityFactory.CreateOrc();
				ork.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(ork);
			    } else if (level == 3)
			    {
				
				Entity ghoul1 = entityFactory.CreateGhoul();
				ghoul1.position = room.Center + new Vector2(Roller.Roll(3), i);
				AddEntity(ghoul1);
				Entity ghoul2 = entityFactory.CreateGhoul();
				ghoul2.position = room.Center + new Vector2(Roller.Roll(3), i + 1);
				AddEntity(ghoul2);
			    }
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
	    Rectangle room = map.GetRoom(p.position);
	    if (!room.IsEmpty)
		AddToRoom(room, player);
	}

	public Vector2 GetPlayerPosition()
	{
	    return player.position;
	}

	public bool Move(Player player, Vector2 dir)
	{
	    Vector2 result = player.position + dir;
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

        public bool PlayerInRoomWithMonster() =>
            !playerInCorridor && roomPopulation[currentRoom].Count > 1;

	public void PlayerPickItem(string name)
	{
	    Vector2 position = player.position;
	    if (map[position].items.Count != 0)
	    {
		Item item = null;
		foreach(Item i in map[position].items.Values)
		{
		    if (i.name == name)
		    {
			i.Pick(player);
			item = i;
		    }
		}
		if (item is not null)
		    map[position].RemoveItem(item);
	    }
	}

	public void AddEntity(Entity entity)
	{
	    Rectangle room = map.GetRoom(entity.position);
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
	    Vector2 result = entity.position + dir;
	    Rectangle room = map.GetRoom(entity.position);
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
	    if (roomPopulation.ContainsKey(currentRoom))
	    {
		foreach (Entity entity in roomPopulation[currentRoom])
		{
		    if (entity is Monster)
			result.Add(entity as Monster);
		}
	    }
	    return result;
	}

	public void PutItem(Item item, Vector2 position)
	{
	    bool found = false;
	    if (!found)
		map[position].PutItem(item);
	}

	public List<Tile> GetLine(Vector2 origin, Vector2 destination)
	{
	    return map.GetLine(
		(int) origin.X, (int) origin.Y, (int) destination.X, (int) destination.Y
	    ).ToList();
	}

	public int GetDistance(Vector2 origin, Vector2 destination)
	{
	    return GetLine(origin, destination).Count - 1;
	}

	public char[,] DrawOnMain()
	{
	    var result = new char[Width, Height];
	    for(int x = 0; x < Width; x++)
	    {
		for(int y = 0; y < Height; y++)
		{
		    result[x, y] = this[x, y].Draw();
		}
	    }
	    if(PlayerInRoomWithMonster())
	    {
		foreach(Monster monster in GetMonstersNearPlayer())
		{
		    result[(int) monster.position.X, (int) monster.position.Y] = monster.Draw();   
		}
	    }
	    result[(int) GetPlayerPosition().X, (int) GetPlayerPosition().Y] = player.Draw();
	    return result;
 	}
	
	public List<string> DrawOnSide()
	{
	    var result = new List<string>();
	    result.Add("Pick an Item");
	    foreach(char key in chooseMap.Keys)
	    {
		string amount = "";
		Item item = chooseMap[key];
		if (item.amount > 1)
		{
		    amount = $"({item.amount})";
		}
		result.Add($"{key} - {item.name} {amount}");
	    }
	    return result;
	}

	private void WriteToLog(string message)
	{
	    gameLog.Add(message);
	}
    }
}
