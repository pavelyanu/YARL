namespace YARL.Drawing
{
    public interface IDrawable
    {
	public char glyph { get; }
	public IDrawBehaviour drawBehaviour { get; }
	public char Draw();
    }
}

