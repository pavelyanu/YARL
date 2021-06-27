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
	    level = new Level(w, h, 9, 15, 5, 1);
	    player = entityFactory.CreatePlayer();
	    player.position = level.Rooms[0].Center;
	    inventoryManager = new InventoryManager(player);
	    level.AddPlayer(player);
	    level.AddEntity(entityFactory.CreateGoblin());
	    Item sword = itemFactory.CreateShortSword();
	    level.PutItem(sword, player.position + new Vector2(0, 1));
	    ended = false;
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
	    if (player.alive)
	    {
		if (!inBattle)
		{
		    if (input[0] == 'k')
			level.Move(player, new Vector2(0, -1));
		    else if(input[0] == 'h')
			level.Move(player, new Vector2(-1, 0));
		    else if(input[0] == 'j')
			level.Move(player, new Vector2(0, 1));
		    else if(input[0] == 'l')
			level.Move(player, new Vector2(1, 0));
		    else if("era".Contains(input[0]))
			inventoryManager.ProcessInput(input[0]);
		    else if(input[0] == ',')
		    {
			level.PlayerPickItem();
		    }
		    if (inBattle)
		    {
			battleManager = new BattleManager(level, player);
		    }
		} else 
		{
		    battleManager.ProcessInput(input[0]);
		}
	    } else 
	    {
		if ("qwertyuiopasdfghjklzxcvbnm".Contains(input[0]))
		{
		    ended = true;
		}
	    }
	    
	}

	public void Draw()
	{
	    main.Clear();	
	    side.Clear();
	    bottom.Clear();
	    level.UpdateView();
	    if (!ended)
	    {
		for (int h = 0; h < Height; h++)
		{
		    for (int w = 0; w < Width; w++)
		    {
			var vector = new Vector2(w, h);
			main.Print(w, h, level[vector].Draw().ToString());
		    }
		}
		main.Print((int) player.position.X, (int) player.position.Y, player.Draw().ToString());
		if (inBattle)
		{
		    foreach(var m in battleManager.monsters)
		    {
			main.Print((int) m.position.X, (int) m.position.Y, m.Draw().ToString());
		    }
		    side.Print(0, 0, battleManager.DrawOnSide());
		    bottom.Print(0, 0, battleManager.DrawOnBottom());
		    if (battleManager.targeting)
		    {
			main.SetBackground(
			    (int) battleManager.cursor.X, (int) battleManager.cursor.Y, Color.Yellow);
		    }
		} else
		{
		side.Print(0, 0, inventoryManager.Draw());
		}
	    } else
	    {
		main.Print(0, 5, "You have died. You can quit the game by clicking on red x in the corner");
	    }
	    
	}
    }
}

