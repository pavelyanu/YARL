using System.Collections.Generic;
using System.Numerics;
using YARL.Drawing;
using YARL.Actions;

namespace YARL.Actors
{
    public abstract class Entity: IDrawable
    {
	public abstract char glyph { get; }
	public IDrawBehaviour drawBehaviour { get; set; }

	public int health { get; protected set; }
	public abstract int movement { get; }
	public abstract int armor_class { get; }
	public Vector2 position { get; protected set; }	
	public Dictionary<string, Action> actions { get; protected set; }
	public bool alive { get => health > 0; }
	
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
