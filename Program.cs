using System;
using Serilog;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using System.Text;

namespace YARL
{
    class Program
    {
	static Console parent;
	static Console child;
	static ContainerConsole container;
	public const int Width = 100;
	public const int Height = 40;
	static YarlGame model;
	static void Main(string[] args)
	{
	    Log.Logger = new LoggerConfiguration()
		.WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
		.CreateLogger();
	    Log.Information("Log start");

	    SadConsole.Game.Create(Width, Height);
	    SadConsole.Game.OnInitialize = Init;
	    SadConsole.Game.OnUpdate = Update;
	    SadConsole.Game.Instance.Run();
	    SadConsole.Game.Instance.Dispose();
	}


	private static void Update(GameTime game)
	{
	    var keys = SadConsole.Global.KeyboardState.KeysPressed; 
	    var sb = new StringBuilder();
	    foreach(var key in keys) {
		sb.Append(key.Character);
	    }
	    string result = sb.ToString();
	    if (!(result is null || result == ""))
	    {
		model.Update(sb.ToString());
		model.Draw();
	    }
	    SadConsole.Global.CurrentScreen = parent;
	}

	private static void Init()
	{
	    parent = new Console(Width, Height);
	    parent.Position = new Point(0, 0);
	    child = new Console(5, 5);
	    child.Fill(null, Color.Red, null);
	    child.Parent = parent;
	    parent.Components.Add(new MouseMoveComponent());
	    SadConsole.Global.CurrentScreen = parent;
	    model = new YarlGame(Width, Height);
	    model.SetConsole(parent);
	}
    }
}
