using System;
using System.Collections.Generic;
using System.Numerics;
using Serilog;
using SadConsole;
using YARL.Topography;
using YARL.Actors;

namespace YARL.Core {
    class YarlGame
    {
	Map current;
	MapGenerator mapGenerator;
	Player player;
	protected int Height;
	protected int Width;
	SadConsole.Console console;

	public YarlGame(int w, int h)
	{
	    Height = h;
	    Width = w;
	    mapGenerator = new MapGenerator(w, h, 9, 15, 5, 1);
	    current = mapGenerator.CreateMap();
	    player = new Player(current.Rooms[0].Center);

	}

	public void  SetConsole(SadConsole.Console c)
	{
	    console = c; 
	}

	public void Update(String input)
	{
	    
	    if (input[0] == 'w')
		Move(player, new Vector2(0, -1));
	    else if(input[0] == 'a')
		Move(player, new Vector2(-1, 0));
	    else if(input[0] == 's')
		Move(player, new Vector2(0, 1));
	    else if(input[0] == 'd')
		Move(player, new Vector2(1, 0));
	}

	public void Draw()
	{
	    for (int h = 0; h < Height; h++)
	    {
		for (int w = 0; w < Width; w++)
		{
		    var vector = new Vector2(w, h);
		    console.Print(w, h, current[vector].Draw().ToString());
		}
	    }
	    console.Print((int) player.position.X, (int) player.position.Y, player.Draw().ToString());
	}

	protected void Move(Entity entity, Vector2 direction)
	{
	    Vector2 result = entity.position + direction;
	    if (current[result].walkable)
		entity.Move(direction);
	}
    }
}

