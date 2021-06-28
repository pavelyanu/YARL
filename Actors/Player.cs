using YARL.Core;
using YARL.Drawing;

namespace YARL.Actors
{
    public class Player: Entity
    {
	public int gold { get; protected set; }
	public Inventory inventory;
	
	public Player(
	    IDrawBehaviour _drawBehaviour,
	    int _str,
	    int _dex,
	    int _inte,
	    int _health
	) : base('@',_drawBehaviour, 6, "You", _str, _dex, _inte, _health)
	{
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
