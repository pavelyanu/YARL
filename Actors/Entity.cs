using System;
using System.Numerics;
using YARL.Drawing;

namespace YARL.Actors
{
    public abstract class Entity: Drawable
    {
	public int health { get; protected set; }
	public int movement { get; protected set; }
	public int armor_class { get; protected set; }
	public Vector2 position { get; protected set; }	
	public abstract void Move(Vector2 direction);
    }
}
