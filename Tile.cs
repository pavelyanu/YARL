using System.Numerics;
using YARL;

namespace YARL
{
    public abstract class Tile: IDrawable
    {
	public abstract Vector2 position { get; protected set; } 
	public abstract bool walkable { get; protected set; }
	public abstract char Draw();
    }
}

