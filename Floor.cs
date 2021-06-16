using System.Numerics;

namespace YARL{
    class Floor: Tile
    {
	public override Vector2 position { get; protected set; }
	public override bool walkable {get => true; protected set => walkable = value;}

	public Floor(Vector2 vector)
	{
	    position = vector;
	}

	public Floor(int x, int y)
	{
	    position = new Vector2(x, y);
	}

	public override char Draw()
	{
	    return '.';
	}
    }
}

