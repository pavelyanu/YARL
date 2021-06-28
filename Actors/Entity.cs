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
	public int armor_class { get => 10 + dex + ac_modifier; }
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
	}

	public override bool Equals(object o)
	{
	    if (o is Entity)
	    {
		var e = o as Entity;
		if (e.name == name && e.position == position)
		{
		    return true;
		}
	    }
	    return false;
	}

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

	public void AddArmour(int ac)
	{
	    ac_modifier = ac;
	}

	public void RemoveArmour()
	{
	    ac_modifier = 0;
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
