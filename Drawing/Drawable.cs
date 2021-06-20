namespace YARL.Drawing
{
    public interface IDrawable
    {
	public char glyph { get; }
	IDrawingBehaviour drawingBehaviour { get; }
	public char Draw();
	public void SetDrawingBehaviour(IDrawingBehaviour b);
	
    }
}

