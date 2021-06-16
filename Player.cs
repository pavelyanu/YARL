using System;
using System.Numerics;

namespace YARL
{
    class Player: Entity
    {
	public override int health { get; protected set; }
	public override int movement { get; protected set; }
	public override int armor_class { get; protected set; }
	public override Vector2 position { get; protected set; }
	
	public Player(Vector2 vector)
	{
	    health = 1;
	    movement = 1;
	    armor_class = 1;
	    position = vector;
	}

	public override char Draw()
	{
	    return '@';
	}

	public override void Move(Vector2 direction)
	{
	    position += direction;    
	}
    }
}
