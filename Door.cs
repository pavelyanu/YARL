using System.Numerics;

namespace YARL
{
    class Door : Tile
    {
	public override Vector2 position { get; protected set; }
	public override bool walkable {get; protected set;}
	public Door(Vector2 vector)
	{
	    position = vector;
	    walkable = true;
	}

	public Door(int x, int y)
	{
	    position = new Vector2(x, y);
	}

	public override char Draw()
	{
	    return '+';
	}
    }
}
