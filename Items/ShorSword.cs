using System.Numerics;
using YARL.Actors;
using YARL.Actions;
using YARL.Drawing;

namespace YARL.Items
{
    class ShortSwordPick: Pickable 
    {
	public override char glyph { get => '\''; }
	public override string name { get => "Short sword"; }

	public ShortSwordPick(Vector2 pos)
	{
	    position = pos;
	}

    }

    class ShortSwordPossession: Equipable
    {
	public override string name { get => "Short sword"; }	
	
	public ShortSwordPossession()
	{
	    behaviour = new AddActionBehaviour(new ShortSwordAttack());
	}
	    
	    

    }
}
