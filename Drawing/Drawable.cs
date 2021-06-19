namespace YARL.Drawing
{
    public abstract class Drawable
    {
	public char glyph { get; protected set; }
	IDrawingBehaviour behaviour;
	public char Draw()
	{
	    if (behaviour is null)
		behaviour = new DefaultDraw();
	    return behaviour.Draw(glyph);
	}
	
	public void SetDrawingBehaviour(IDrawingBehaviour b)
	{
	    behaviour = b;
	}
	
    }
}

