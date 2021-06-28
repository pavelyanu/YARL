using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Serilog;
using SadConsole;
using YARL.Topography;
using YARL.Actors;
using YARL.Drawing;
using YARL.Items;

using Vector2 = System.Numerics.Vector2;

namespace YARL.Core {
    class YarlGame
    {
	protected int Height;
	protected int Width;
	bool inBattle { get => level.PlayerInRoomWithMonster(); }
	bool ended;
	bool showInventory;
	Level level;
	InventoryManager inventoryManager;
	BattleManager battleManager;
	Player player;
	ItemFactory itemFactory;
	EntityFactory entityFactory;
	Console main;
	Console side;
	Console bottom;

	public YarlGame(int w, int h)
	{
	    Height = h;
	    Width = w;
	    itemFactory = new ItemFactory(new DefaultDraw());
	    entityFactory = new EntityFactory(new DefaultDraw());
	    level = new Level(w, h, 12, 15, 5, 1);
	    player = entityFactory.CreatePlayer();
	    player.position = level.Rooms[0].Center;
	    inventoryManager = new InventoryManager(player);
	    level.AddPlayer(player);
	    ended = false;
	    showInventory = false;
	}

	public void  SetConsoles(
	    Console _main, Console _side, Console _bottom
	)
	{
	    main = _main; 
	    side = _bottom;
	    bottom = _side;
	}

	public void Update(string input)
	{
	    char key = input[0];
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
			battleManager = new BattleManager(level, player);
		    }
		} else 
		{
		    if (inventoryManager.selecting || "eru".Contains(key))
		    {
			inventoryManager.ProcessInput(key);
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
	    
	}

	public void Draw()
	{
	    Log.Information("Reached Draw");
	    main.Clear();	
	    side.Clear();
	    bottom.Clear();
	    level.UpdateView();
	    Log.Information("Cleared conosles and updated the view");
	    if (level.playerHasWon)
	    {
		main.Print(0, 5, "You have found the gem and killed all monsters. You have won!");
	    } else if (!ended)
	    {
		for (int h = 0; h < Height; h++)
		{
		    for (int w = 0; w < Width; w++)
		    {
			var vector = new Vector2(w, h);
			main.Print(w, h, level[vector].Draw().ToString());
		    }
		}
		Log.Information("Managed to print to main");
		main.Print((int) player.position.X, (int) player.position.Y, player.Draw().ToString());
		if (inBattle)
		{
		    foreach(var m in battleManager.monsters)
		    {
			main.Print((int) m.position.X, (int) m.position.Y, m.Draw().ToString());
		    }
		    if (inventoryManager.selecting)
		    {
			DrawListOnConsole(inventoryManager.DrawOnSide(), side);
			DrawListOnConsole(inventoryManager.DrawOnBottom(), bottom);
		    } else
		    {
			DrawListOnConsole(battleManager.DrawOnSide(), side);
			DrawListOnConsole(battleManager.DrawOnBottom(), bottom);
		    }
		    if (battleManager.targeting)
		    {
			main.SetBackground(
			    (int) battleManager.cursor.X, (int) battleManager.cursor.Y, Color.Yellow);
		    }
		} else if (inventoryManager.selecting)
		{
		    DrawListOnConsole(inventoryManager.DrawOnSide(), side);
		    DrawListOnConsole(inventoryManager.DrawOnBottom(), bottom);
		} else if (level.choosingItem)
		{
		    DrawListOnConsole(level.DrawOnSide(), side);
		    DrawListOnConsole(level.DrawOnBottom(), bottom);
		} else if (showInventory)
		{
		    DrawListOnConsole(inventoryManager.DrawOnSide(), side);
		}
	    } else
	    {
		main.Print(0, 5, "You have died. You can quit the game by clicking on red x in the corner");
	    }
	    Log.Information("Finished Draw");
	}

	public void DrawListOnConsole(List<string> list, Console console)
	{
	    if (list is not null && list.Count != 0)
	    {
		console.Clear();
		for(int i = 0; i < list.Count; i++)
		{
		    console.Print(0, i, list[i]);
		}
	    }
	}
    }
}

