using System.Collections.Generic;
using Environment = System.Environment;
using SadConsole;
using YARL.Actors;
using YARL.Drawing;

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
	CharacterManager characterManager;
	public BattleManager battleManager;
	Player player;
	EntityFactory entityFactory;
	GameLog gameLog;

	public YarlGame(int w, int h, int logSize)
	{
	    Height = h;
	    Width = w;
	    gameLog = new GameLog(logSize - 3);
	    entityFactory = new EntityFactory(new DefaultDraw());
	    player = entityFactory.CreatePlayer();
	    player.levelTransfer += OnLevelTransfer;
	    level = new Level(w, h, 12, 15, 5, 1, gameLog, player);
	    inventoryManager = new InventoryManager(player, gameLog);
	    characterManager = new CharacterManager(player, gameLog);
	    ended = false;
	    showInventory = true;
	    if (inBattle)
	    {
		battleManager = new BattleManager(level,player, gameLog);
	    }
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
		    if (characterManager.leveling)
		    {
			characterManager.ProcessInput(key);
		    } else if (level.choosingItem)
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
		    } else if (key == ',')
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

	public void OnLevelTransfer()
	{
	    level = new Level(Width, Height, 12, 15, 5, 2, gameLog, player);
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
	    } else if (characterManager.leveling)
	    {
		var result = characterManager.Draw();
		result[characterManager.selectedLine] = result[characterManager.selectedLine].ToUpper();
		return result;
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

