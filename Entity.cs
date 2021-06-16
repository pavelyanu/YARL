using System;
using System.Numerics;

namespace YARL
{
    abstract class Entity: IDrawable
    {
	public abstract int health { get; protected set; }
	public abstract int movement { get; protected set; }
	public abstract int armor_class { get; protected set; }
	public abstract Vector2 position { get; protected set; }	
	public abstract char Draw();
	public abstract void Move(Vector2 direction);
    }
}
