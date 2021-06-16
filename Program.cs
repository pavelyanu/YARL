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
	static Console console;
	static ContainerConsole container;
	public const int Width = 100;
	public const int Height = 40;
	static YarlGame model;
        static void Main(string[] args)
        {
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
	    console = model.Draw();
	    SadConsole.Global.CurrentScreen = console;
	    }
	}

	private static void Init()
	{
	    console = new Console(Width, Height);
	    SadConsole.Global.CurrentScreen = console;
	    model = new YarlGame(Width, Height);
	}
    }
}
