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
	bool inBattle { get => current.PlayerInRoomWithMonster(); }
	Level current;
	InventoryManager inventoryManager;
	BattleManager battleManager;
	Player player;
	ItemFactory itemFactory;
	Console main;
	Console side;
	Console bottom;

	public YarlGame(int w, int h)
	{
	    Height = h;
	    Width = w;
	    current = new Level(w, h, 9, 15, 5, 1);
	    player = new Player(current.Rooms[0].Center);
	    player.drawBehaviour = new DefaultDraw();
	    itemFactory = new ItemFactory();
	    current.AddPlayer(player);
	    inventoryManager = new InventoryManager(player);
	    Monster monster = new Monster(current.Rooms[1].Center);
	    monster.drawBehaviour = new DefaultDraw();
	    current.AddEntity(monster);
	    Item sword = itemFactory.CreateShortSword();
	    current.PutItem(sword, player.position + new Vector2(0, 1));
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
	    if (!inBattle)
	    {
		Log.Information("Player is not in battle");		
		if (input[0] == 'k')
		    current.Move(player, new Vector2(0, -1));
		else if(input[0] == 'h')
		    current.Move(player, new Vector2(-1, 0));
		else if(input[0] == 'j')
		    current.Move(player, new Vector2(0, 1));
		else if(input[0] == 'l')
		    current.Move(player, new Vector2(1, 0));
		else if(input[0] == 'e')
		    inventoryManager.ProcessInput('e');
		else if(input[0] == 'r')
		    inventoryManager.ProcessInput('r');
		else if(input[0] == 'a')
		    inventoryManager.ProcessInput('a');
		else if(input[0] == ',')
		{
		    current.PlayerPickItem();
		}
		if (inBattle)
		{
		    Log.Information("Player has entered the room with monsters");
		    battleManager = new BattleManager(current, player);
		}
	    } else 
	    {
		Log.Information("Player is in battle!");		
		battleManager.ProcessInput(input[0]);
	    }
	}

	public void Draw()
	{
	    main.Clear();	
	    side.Clear();
	    bottom.Clear();
	    current.UpdateView();
	    for (int h = 0; h < Height; h++)
	    {
		for (int w = 0; w < Width; w++)
		{
		    var vector = new Vector2(w, h);
		    main.Print(w, h, current[vector].Draw().ToString());
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
	}
    }
}

