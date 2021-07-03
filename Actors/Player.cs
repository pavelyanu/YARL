using System.Collections.Generic;
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

	public List<string> DrawStats()
	{
	    var result = new List<string>();
	    result.Add($"str. : {str}");
	    result.Add($"dex. : {dex}");
	    result.Add($"int. : {inte}");
	    return result;
	}

	public List<string> DrawInfo()
	{
	    var result = new List<string>();
	    result.Add($"health : {health}");
	    result.Add($"ac. : {armor_class}");
	    return result;
	}
    }
}
