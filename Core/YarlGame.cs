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
	Console main;
	Console side;
	Console bottom;

	public YarlGame(int w, int h)
	{
	    Height = h;
	    Width = w;
	    current = new Level(w, h, 9, 15, 5, 1);
	    player = new Player(current.Rooms[0].Center);
	    player.SetDrawingBehaviour(new DefaultDraw());
	    current.AddPlayer(player);
	    inventoryManager = new InventoryManager(player);
	    Monster monster = new Monster(current.Rooms[1].Center);
	    monster.SetDrawingBehaviour(new DefaultDraw());
	    current.AddEntity(monster);
	    var sword = new ShortSwordPick(player.position + new Vector2(0, 1));
	    sword.SetBehaviour(new PossessablePickBehaviour(new ShortSwordPossession()));
	    sword.SetDrawingBehaviour(new DefaultDraw());
	    current.AddItem(sword);
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
			inventoryManager.Equip();
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

