using System;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using SadConsole;
using Serilog;
using Console = SadConsole.Console;
using YARL.Core;

namespace YARL 
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
	public ScrollingConsole MainConsole;
	public Console LeftSideConsole;
	public Console RightSideConsole;
	public Console BottomConsole;
	public Console UpperLeftConsole;
	public Console UpperMiddleConsole;
	public Console UpperRightConsole;
        public Window Window;
	public YarlGame model;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;
	    UseMouse = false;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
        }

        public void CreateConsoles(int W, int H, int _X, int _Y)
        {
	    Width = W;
	    Height = H;

	    int X;
	    int Y;
	    double WRatio;
	    double HRatio;

	    // Set UpperLeftConsole
	    X = _X;
	    Y = _Y;
	    WRatio =  0.3;
	    HRatio = 0.25;
	    
	    UpperLeftConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    UpperLeftConsole.Position = new Point(X, Y);


	    // Set UpperMiddleConsole
	    X = UpperLeftConsole.Width;
	    Y = _Y;
	    WRatio =  0.3;
	    HRatio = 0.25;
	    
	    UpperMiddleConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    UpperMiddleConsole.Position = new Point(X, Y);


	    // Set UpperRightConsole
	    X = UpperMiddleConsole.Position.X + UpperMiddleConsole.Width;
	    Y = _Y;
	    WRatio =  0.3;
	    HRatio = 0.25;
		
	    UpperRightConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    UpperRightConsole.Position = new Point(X, Y);

	    
	    // Set LeftSideConsole
	    X = _X;
	    Y = UpperLeftConsole.Height;
	    WRatio =  0.2;
	    HRatio = 0.5;
		
	    LeftSideConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    LeftSideConsole.Position = new Point(X, Y);
	    UpperLeftConsole.Fill(Color.Pink, Color.Pink, 0);

	    
	    // Set MainConsole
	    W  = (int) Math.Floor((double) (W * 0.6));
	    H = (int) Math.Floor((double) (H * 0.5));
	    MainConsole = new ScrollingConsole(W, H);
	    MainConsole.Position = new Point(LeftSideConsole.Position.X + LeftSideConsole.Width,
				    UpperLeftConsole.Height);
		

	    // Set RightSideConsole
	    X = MainConsole.Position.X + MainConsole.Width;
	    Y = UpperLeftConsole.Height;
	    WRatio =  0.2;
	    HRatio = 0.5;
		
	    RightSideConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    RightSideConsole.Position = new Point(X, Y);
	    UpperLeftConsole.Fill(Color.Yellow, Color.Yellow, 0);


	    // Set BottomConsole
	    X = _X;
	    Y = MainConsole.Position.Y + MainConsole.Height;
	    WRatio =  1;
	    HRatio = 0.25;

	    BottomConsole = new Console(
		(int) Math.Floor(Width * WRatio),
		(int) Math.Floor(Height * HRatio)
	    );
	    BottomConsole.Position = new Point(X, Y);
        }

	public void SetModel(YarlGame _model)
	{
	    model = _model;
	}

        // centers the viewport camera on an Actor
        public void CenterOnActor(System.Numerics.Vector2 p)
        {
            MainConsole.CenterViewPortOnPoint(new Point((int) p.X, (int) p.Y));
        }

        public override void Update(TimeSpan timeElapsed)
        {
            CheckKeyboard();
	    Redraw();
            base.Update(timeElapsed);
        }

        // Scans the SadConsole's Global KeyboardState and triggers behaviour
        // based on the button pressed.
        private void CheckKeyboard()
        {
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Up))
            {
		model.Update("k");
                CenterOnActor(model.level.GetPlayerPosition());
            }else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Down))
            {
		model.Update("j");
                CenterOnActor(model.level.GetPlayerPosition());
            }else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Left))
            {
		model.Update("h");
                CenterOnActor(model.level.GetPlayerPosition());
            }else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Right))
            {
		model.Update("l");
                CenterOnActor(model.level.GetPlayerPosition());
            } else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Enter))
	    {
		model.Update("\n");
	    } else if (SadConsole.Global.KeyboardState.IsKeyPressed(Keys.Escape))
	    {
		model.Update("\r");
	    } else 
	    {
		var keys = SadConsole.Global.KeyboardState.KeysPressed;
		if (keys.Count != 0)
		{
		    model.Update(keys[0].Character.ToString());
		    CenterOnActor(model.level.GetPlayerPosition());
		}
	    }

        }

	public override bool ProcessMouse(SadConsole.Input.MouseConsoleState state)
	{
	    return true;
	}

	public void Redraw()
	{
	    MainConsole.Clear();
	    UpperLeftConsole.Clear();
	    UpperMiddleConsole.Clear();
	    UpperRightConsole.Clear();
	    LeftSideConsole.Clear();
	    RightSideConsole.Clear();
	    BottomConsole.Clear();
	    if (model.level.playerHasWon)
	    {
		MainConsole.Print(0, MainConsole.Height / 2, "You have won. Congratulations!");
	    } else if (model.ended)
	    {
		MainConsole.Print(0, MainConsole.Height / 2, "You have died...");
	    } else 
	    {
		var array = model.DrawOnMain();
		for (int x = 0; x < array.GetLength(0); x++)
		{
		    for (int y = 0; y < array.GetLength(1); y++)
		    {
			MainConsole.SetGlyph(x, y, (int) array[x, y]);
		    }
		}
		if (model.inBattle && model.battleManager.targeting)
		{
		    var cursor = model.battleManager.cursor;
		    int x = (int) cursor.X;
		    int y = (int) cursor.Y;
		    MainConsole.SetGlyph(x, y, (int) array[x, y], Color.Black, Color.Yellow);
		}
	    }

	    model.DrawListOnConsole(model.DrawOnUpperLeft(), UpperLeftConsole);
	    model.DrawListOnConsole(model.DrawOnUpperMiddle(), UpperMiddleConsole);
	    model.DrawListOnConsole(model.DrawOnUpperRight(), UpperRightConsole);
	    model.DrawListOnConsole(model.DrawOnLeftSide(), LeftSideConsole);
	    model.DrawListOnConsole(model.DrawOnRightSide(), RightSideConsole);
	    model.DrawListOnConsole(model.DrawOnBottom(), BottomConsole);
	    DrawLinesAroundConsole(RightSideConsole);
	    DrawLinesAroundConsole(LeftSideConsole);
	    DrawLinesAroundConsole(UpperRightConsole);
	    DrawLinesAroundConsole(UpperMiddleConsole);
	    DrawLinesAroundConsole(UpperLeftConsole);
	    DrawLinesAroundConsole(BottomConsole);
	}

        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
	
	void DrawLinesAroundConsole(Console console)
	{
	    console.DrawBox(
		new Rectangle(
		    0,
		    0,
		    console.Width,
		    console.Height),
		new Cell(Color.White, Color.Black, 7));
	}

        public void CreateWindow(int width, int height, string title)
        {
            int MainConsoleWidth = width - 2;
            int MainConsoleHeight = height - 2;

	    CreateConsoles(MainConsoleWidth, MainConsoleHeight, 1, 1);

            //add the map viewer to the window
            Children.Add(MainConsole);
            Children.Add(UpperLeftConsole);
            Children.Add(UpperMiddleConsole);
            Children.Add(UpperRightConsole);
            Children.Add(LeftSideConsole);
            Children.Add(RightSideConsole);
            Children.Add(BottomConsole);
        }
    }
}
