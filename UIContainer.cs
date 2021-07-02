using System;
using Vector2 = System.Numerics;
using Microsoft.Xna.Framework;
using SadConsole;
using YARL.Core;

namespace YARL 
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
	public ScrollingConsole MainConsole;
	public ControlsConsole LeftSideConsole;
	public ControlsConsole RightSideConsole;
	public ControlsConsole BottomConsole;
	public ControlsConsole LeftUpperConsole;
	public ControlsConsole MiddleUpperConsole;
	public ControlsConsole RightUpperConsole;
        public Window Window;
	public YarlGame model;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
        }

        // Creates all child consoles to be managed
        public void CreateConsoles(int W, int H, int X, int Y)
        {
	    Width = W;
	    Height = H;
            // Temporarily create a console with *no* tile data that will later be replaced with map data
	    SetControlsConsole(
		console: LeftUpperConsole,
		X: X,
		Y: Y,
		WRatio: 0.3,
		HRatio: 0.25
	    );
		
	    SetControlsConsole(
		console: MiddleUpperConsole,
		X: LeftUpperConsole.Width,
		Y: Y,
		WRatio: 0.3,
		HRatio: 0.25
	    );
		
	    SetControlsConsole(
		console: RightUpperConsole,
		X: MiddleUpperConsole.Position.X + MiddleUpperConsole.Width,
		Y: Y,
		WRatio: 0.3,
		HRatio: 0.25
	    );
		
	    SetControlsConsole(
		console: LeftSideConsole,
		X: X,
		Y: LeftSideConsole.Height,
		WRatio: 0.2,
		HRatio: 0.5
	    );
		
	    MainConsole = new ScrollingConsole(W, H);
	    W  = (int) Math.Floor((double) (W * 0.6));
	    H = (int) Math.Floor((double) (H * 0.5));
	    MainConsole.Position = new Point(LeftSideConsole.Position.X + LeftSideConsole.Height,
				    LeftUpperConsole.Height);
		
	    SetControlsConsole(
		console: RightSideConsole,
		X: MainConsole.Position.X + MainConsole.Width,
		Y: LeftSideConsole.Height,
		WRatio: 0.2,
		HRatio: 0.5
	    );
		
	    SetControlsConsole(
		console: BottomConsole,
		X: X,
		Y: MainConsole.Position.Y + MainConsole.Height,
		WRatio: 1,
		HRatio: 0.25
	    );
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
            base.Update(timeElapsed);
        }

        // Scans the SadConsole's Global KeyboardState and triggers behaviour
        // based on the button pressed.
        private void CheckKeyboard()
        {
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            // Decrement player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
		model.Update("k");
                CenterOnActor(model.level.GetPlayerPosition());
            }

            // Keyboard movement for Player character: Down arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
		model.Update("k");
                CenterOnActor(model.level.GetPlayerPosition());
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
		model.Update("k");
                CenterOnActor(model.level.GetPlayerPosition());
            }

            // Keyboard movement for Player character: Right arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
		model.Update("k");
                CenterOnActor(model.level.GetPlayerPosition());
            }

        }

	private void SetControlsConsole(ControlsConsole console, int X, int Y, double WRatio, double HRatio) 
	{
	    console = new ControlsConsole((int) Math.Floor(Width * WRatio), (int) Math.Floor(Height * HRatio));
	    console.Position = new Point(X, Y);
	}

        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
        public void CreateWindow(int width, int height, string title)
        {
            Window = new Window(width, height);
            Window.CanDrag = true;

            int MainConsoleWidth = width - 2;
            int MainConsoleHeight = height - 2;

            // Resize the Map Console's ViewPort to fit inside of the window's borders snugly
            Window.ViewPort = new Rectangle(0, 0, MainConsoleWidth, MainConsoleHeight);

            //reposition the MapConsole so it doesnt overlap with the left/top window edges
            Window.Position = new Point(1, 1);

            // Centre the title text at the top of the window
            Window.Title = title.Align(HorizontalAlignment.Center, MainConsoleWidth);

            //add the map viewer to the window
            Window.Children.Add(MainConsole);
            Window.Children.Add(LeftUpperConsole);
            Window.Children.Add(MiddleUpperConsole);
            Window.Children.Add(RightUpperConsole);
            Window.Children.Add(LeftSideConsole);
            Window.Children.Add(RightSideConsole);
            Window.Children.Add(BottomConsole);

            // The MapWindow becomes a child console of the UIManager
            Children.Add(Window);

            // Without this, the window will never be visible on screen
            Window.Show();
        }
    }
}
