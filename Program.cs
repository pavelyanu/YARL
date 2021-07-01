using Serilog;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using System.Text;
using YARL.Core;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace YARL
{
    class Program
    {
	static Console main;
	static Console bottom;
	static Console side;
	public const int Width = 100;
	public const int Height = 40;
	public const int InvWidth = 30;
	public const int BatHeight = 5;
	static YarlGame model;
	static void Main(string[] args)
	{
	    Log.Logger = new LoggerConfiguration()
		.WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
		.CreateLogger();
	    Log.Information("Log start");

	    SadConsole.Game.Create(Width + InvWidth, Height + BatHeight);
	    SadConsole.Game.OnInitialize = Init;
	    SadConsole.Game.OnUpdate = Update;
	    SadConsole.Game.Instance.Run();
	    SadConsole.Game.Instance.Dispose();
	}


	private static void Update(GameTime game)
	{
	    if (SadConsole.Global.KeyboardState.IsKeyDown(Keys.Enter))
	    {
		Log.Information($"Enter has been pressed");
		model.Update('\n'.ToString());
	    } else if (SadConsole.Global.KeyboardState.IsKeyDown(Keys.Escape))
	    {
		Log.Information($"Escape has been pressed");
		model.Update('\r'.ToString());
	    } else {
		var keys = SadConsole.Global.KeyboardState.KeysPressed; 
		
		var sb = new StringBuilder();
		foreach(var key in keys) {
		    sb.Append(key.Character);
		}
		string result = sb.ToString();
		if (!(result is null || result == ""))
		{
		    Log.Information($"{result} has been pressed");
		    model.Update(sb.ToString());
		}
	    }
	    model.Draw();
	    SadConsole.Global.CurrentScreen = main;
	}

	private static void Init()
	{
	    main = new Console(Width, Height);
	    main.Position = new Point(0, 0);
	    bottom = new Console(InvWidth, Height);
	    bottom.Position = new Point(Width, 0);
	    bottom.Parent = main;
	    side = new Console(Width, BatHeight);
	    side.Position = new Point(0, Height);
	    side.Fill(null, Color.Blue, null);
	    side.Parent = main;
	    SadConsole.Global.CurrentScreen = main;
	    model = new YarlGame(Width, Height);
	    model.SetConsoles(main, side, bottom);
	}
    }
}
