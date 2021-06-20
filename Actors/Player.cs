using System.Collections.Generic;
using System.Numerics;
using YARL.Actions;
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
	public Dictionary<string, List<Equipable>> possessions;
	
	public Player(Vector2 vector)
	{
	    health = 10;
	    position = vector;
	    possessions = new Dictionary<string, List<Equipable>>();
	    actions = new Dictionary<string, Action>();
	}

	public void AddPossession(Equipable possession)
	{
	    if (!possessions.ContainsKey(possession.name))
		possessions[possession.name] = new List<Equipable>();

	    possessions[possession.name].Add(possession);
	}

	public void RemovePossession(Equipable possession)
	{
	    if (possessions.ContainsKey(possession.name))
		possessions[possession.name].RemoveAt(0);

	    if (possessions[possession.name].Count == 0)
		possessions.Remove(possession.name);
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
