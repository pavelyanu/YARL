using System.Numerics;

namespace YARL{
    class Wall: Tile
    {
	public override Vector2 position { get; protected set; }
	public override bool walkable { get => false; protected set => walkable = value; }

	public Wall(Vector2 vector)
	{
	    position = vector;
	}

	public Wall(int x, int y)
	{
	    position = new Vector2(x, y);
	}

	public override char Draw()
	{
	    return ' ';
	}
    }
}

