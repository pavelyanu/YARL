using System.Collections.Generic;
using System.Numerics;
using YARL.Actions;
using YARL.Items;

namespace YARL.Actors
{
    public class Monster: Entity
    {
	public override int movement { get => 30; }
	public override int armor_class { get => 10; }
	public override char glyph { get => 'g';}
	public override string name { get => "monster"; }
	
	public Monster(Vector2 vector)
	{
	    health = 10;
	    position = vector;
	}
    }
}
