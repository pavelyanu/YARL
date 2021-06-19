using System;
using System.Numerics;

namespace YARL.Actors
{
    public class Player: Entity
    {
	public int gold { get; protected set; }
	
	public Player(Vector2 vector)
	{
	    health = 1;
	    movement = 1;
	    armor_class = 1;
	    position = vector;
	    glyph = '@';
	}

	public override void Move(Vector2 direction)
	{
	    position += direction;    
	}
    }
}
