using Serilog;
using Microsoft.Xna.Framework;
using YARL.Core;

namespace YARL
{
    class Program
    {
	public const int Width = 200;
	public const int Height = 60;
	static YarlGame model;
	static UIManager manager;
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
	}

	private static void Init()
	{
	    manager = new UIManager();    
	    manager.CreateWindow(Width, Height, "YARL");
	    model = new YarlGame(
		manager.MainConsole.Width,
		manager.MainConsole.Height,
		manager.BottomConsole.Height
	    );
	    manager.SetModel(model);
	}
    }
}
