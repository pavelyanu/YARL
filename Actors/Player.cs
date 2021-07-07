using System.Collections.Generic;
using YARL.Core;
using YARL.Drawing;

namespace YARL.Actors
{
    public class Player: Entity
    {
	public int gold { get; protected set; }
	public int exp { get; protected set; }
	public int nextLvl { get; protected set; }
	public int lvl { get; protected set; }

	public delegate void LeveledUp(int level);
	public event LeveledUp leveledUp;

	public Player(
	    IDrawBehaviour _drawBehaviour,
	    int _str,
	    int _dex,
	    int _inte,
	    int _health
	) : base('@',_drawBehaviour, 6, "You", _str, _dex, _inte, _health)
	{
	    inventory = new PlayerInventory(this);
	}

	public void AddExp(int _exp)
	{
	    exp += _exp;
	    if (exp > nextLvl)
	    {
		exp = exp - nextLvl;
		lvl += 1;
		leveledUp(lvl);
	    }
	}

	public void SetStr(int _str)
	{
	    str = _str;
	}

	public void SetDex(int _dex)
	{
	    dex = _dex;
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
