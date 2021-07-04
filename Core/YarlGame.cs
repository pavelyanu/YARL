using System.Collections.Generic;
using Environment = System.Environment;
using SadConsole;
using YARL.Actors;
using YARL.Drawing;
using YARL.Items;

namespace YARL.Core {
    public class YarlGame
    {
	protected int Height;
	protected int Width;
	public bool inBattle { get => level.PlayerInRoomWithMonster(); }
	public bool playerHasLost { get => !player.alive; }
	public bool ended;
	bool showInventory;
	public Level level;
	InventoryManager inventoryManager;
	public BattleManager battleManager;
	Player player;
	ItemFactory itemFactory;
	EntityFactory entityFactory;
	GameLog gameLog;

	public YarlGame(int w, int h, int logSize)
	{
	    Height = h;
	    Width = w;
	    gameLog = new GameLog(logSize - 3);
	    itemFactory = new ItemFactory(new DefaultDraw());
	    entityFactory = new EntityFactory(new DefaultDraw());
	    level = new Level(w, h, 12, 15, 5, 1, gameLog);
	    player = entityFactory.CreatePlayer();
	    player.position = level.Rooms[0].Center;
	    inventoryManager = new InventoryManager(player, gameLog);
	    level.AddPlayer(player);
	    ended = false;
	    showInventory = true;
	}

	public void Update(string input)
	{
	    char key = input[0];
	    if (key == 'Q')
	    {
		Environment.Exit(0);
	    }
	    if (player.alive)
	    {
		if (!inBattle)
		{
		    if (level.choosingItem)
		    {
			level.ProcessInput(key);
		    } else if (inventoryManager.selecting || "eru".Contains(key)) 
		    {
			inventoryManager.ProcessInput(key);
		    } else if (key == 'i')
		    {
			showInventory = !showInventory;
		    } else 
		    {
			level.ProcessInput(key);
		    }
		    if (inBattle)
		    {
			battleManager = new BattleManager(level, player, gameLog);
		    }
		} else 
		{
		    if (inventoryManager.selecting || "eru".Contains(key))
		    {
			inventoryManager.ProcessInput(key);
		    } else if (key == 'i')
		    {
			level.ProcessInput(key);
		    } else
		    {
			battleManager.ProcessInput(key);	
		    }
		}
	    } else 
	    {
		if ("qwertyuiopasdfghjklzxcvbnm".Contains(key))
		{
		    ended = true;
		}
	    }
	    level.UpdateView();
	    
	}

	public char[,] DrawOnMain()
	{
	    return level.DrawOnMain();
	}

	public List<string> DrawOnLeftSide()
	{
	    if (inBattle)
	    {
		return battleManager.DrawOnSide();
	    } else if (level.choosingItem)
	    {
		return level.DrawOnSide();
	    }
	    else return new List<string>();
	}

	public List<string> DrawOnRightSide()
	{
	    return inventoryManager.DrawOnSide();
	}

	public List<string> DrawOnBottom()
	{
	    return gameLog.view;
	}

	public List<string> DrawOnUpperLeft()
	{
	    return player.DrawStats();
	}

	public List<string> DrawOnUpperMiddle()
	{
	    return player.DrawInfo();
	}

	public List<string> DrawOnUpperRight()
	{
	    if (inBattle)
	    {
		return battleManager.DrawPlayerInfo();
	    } else return new List<string>();
	}

	public void DrawListOnConsole(List<string> list, Console console)
	{
	    if (list is not null && list.Count != 0)
	    {
		console.Clear();
		for(int i = 0; i < list.Count; i++)
		{
		    console.Print(2, i + 2, list[i]);
		}
	    }
	}
    }
}

