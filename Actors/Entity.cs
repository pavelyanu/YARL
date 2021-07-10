using System.Collections.Generic;
using System.Numerics;
using YARL.Drawing;
using YARL.Actions;

namespace YARL.Actors
{
    public abstract class Entity: IDrawable
    {
	public char glyph { get; set; }
	public IDrawBehaviour drawBehaviour { get; set; }

	public int movement { get; protected set; }
	public int n_of_actions { get; protected set; }
	public int armor_class { get => 10 + GetCappedDex() + ac_modifier; }
	public int dex_ac_cap { get; protected set; }
	public int ac_modifier { get; protected set; }
	public string name { get; protected set; }
	public int str { get; protected set; }
	public int dex { get; protected set; }
	public int inte { get; protected set; }
	public int health { get; protected set; }
	public int maxHealth { get; protected set; }
	public bool alive { get => health > 0; }
	public Vector2 position { get; set; }	
	public Dictionary<string, Action> actions { get; protected set; }
	public Inventory inventory;

	public Entity(
	    char _glyph,
	    IDrawBehaviour _drawBehaviour,
	    int _movement,
	    string _name,
	    int _str,
	    int _dex,
	    int _inte,
	    int _health
	)
	{
	    glyph = _glyph;
	    drawBehaviour = _drawBehaviour;
	    movement = _movement;
	    name = _name;
	    str = _str;
	    dex = _dex;
	    health = _health;
	    maxHealth = _health;
	    actions = new Dictionary<string, Action>();
	    ac_modifier = 0;
	    n_of_actions = 1;
	}

	public override bool Equals(object o) =>
	        o is Entity e && e.name == name && e.position == position;

	public override int GetHashCode()
	{
	    return $"{name}{position.ToString()}".GetHashCode();
	}

	public void Inflict(int damage)
	{
	    health -= damage;	
	}

	public void AddAction(Action action)
	{
	    if (!actions.ContainsKey(action.name))
	    {
		actions[action.name] = action;
	    }
	}

	public void RemoveAction(Action action)
	{
	    actions.Remove(action.name);
	}

	public void AddArmour(int ac, int dex_cap)
	{
	    ac_modifier = ac;
	    dex_ac_cap = dex_cap;
	}

	public void RemoveArmour()
	{
	    ac_modifier = 0;
	}

	int GetCappedDex()
	{
	    if (dex_ac_cap > 0)
	    {
		if (dex > dex_ac_cap)
		{
		    return dex_ac_cap;
		} else return dex;
	    } else return dex;
	}

	public void Move(Vector2 direction)
	{
	    position += direction;    
	}

	public char Draw()
	{
	    return drawBehaviour.Draw(glyph);
	}
    }
}
