using System.Collections.Generic;
using System.Numerics;
using YARL.Actions;
using YARL.Core;
using YARL.Items;

namespace YARL.Actors
{
    public class Player: Entity
    {
	public override int movement { get => 5; }
	public override int armor_class { get => CalculateAC(); }
	public int ac_modifier { get; protected set; }
	public override char glyph { get => '@';}
	public int gold { get; protected set; }
	public Inventory inventory;
	
	public Player(Vector2 vector)
	{
	    health = 10;
	    position = vector;
	    actions = new Dictionary<string, Action>();
	    inventory = new Inventory(this);
	}

	public int CalculateAC()
	{
	    return 10 + ac_modifier;
	}

	public void ModifyAC(int i)
	{
	    ac_modifier += i;
	}
    }
}
