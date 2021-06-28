using SadConsole.Components;
using SadConsole.Input;

namespace YARL
{
    class MouseMoveComponent: MouseConsoleComponent
    {
	public override void ProcessMouse(
	    SadConsole.Console console,
	    MouseConsoleState state,
	    out bool handled
	)
	{
	    if (state.Mouse.IsOnScreen)
		console.Children[0].Print(0, 0, state.WorldCellPosition.ToString());

	    handled = true;
	}
    }
}
    
