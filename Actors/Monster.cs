using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Serilog;
using YARL.Actions;
using YARL.Core;
using YARL.Drawing;
using YARL.Items;
using YARL.Topography;

namespace YARL.Actors
{
    public class Monster: Entity
    {
	public int exp { get; protected set; }
	public double lootChance { get; set; }			
	public Monster(
	    char _glyph,
	    IDrawBehaviour _drawBehaviour,
	    int _movement,
	    string _name,
	    int _str,
	    int _dex,
	    int _inte,
	    int _health,
	    int _exp,
	    double _lootchance,
	    Action _action
	    ) : base(_glyph, _drawBehaviour, _movement, _name, _str, _dex, _inte, _health)
	{
	    exp = _exp;
	    lootChance = _lootchance;
	    actions.Add(_action.name, _action);
	    inventory = new Inventory();
	} 
	public (List<Vector2>, Action) MakeMove(Level level)
	{
	    Log.Information($"Starting to plan the move");
	    var vectors = new List<Vector2>();    
	    Action action = actions.Values.ToList()[0];
	    Log.Information($"Action is {action.name}");
	    int distance = level.GetDistance(position, level.GetPlayerPosition()); 
	    Log.Information($"My position is {position}");
	    Log.Information($"The player position is {level.GetPlayerPosition()}");
	    Log.Information($"The distance to the player is {distance}");
	    if (distance > action.range){
		Log.Information("The distance is too big");
		List<Tile> tilesToPlayer = level.GetLine(position, level.GetPlayerPosition());
		tilesToPlayer.RemoveAt(0);
		tilesToPlayer.RemoveAt(tilesToPlayer.Count - 1);
		foreach(Tile tile in tilesToPlayer)
		{
		    Log.Information($"The tile {tile.position} is on the way to player");
		}
		Log.Information($"First tile of the way to player is {tilesToPlayer[0].position}");
		vectors.Add(tilesToPlayer[0].position - position);
		Vector2 previousPosition = position + vectors[0];
		Log.Information($"Vector {vectors[0]} will lead to it");
		tilesToPlayer.RemoveAt(0);
		if (tilesToPlayer.Count != 0)
		{
		    for(int i = 1; i < movement; i++)
		    {
			Log.Information($"Next tile of the way to player is {tilesToPlayer[0].position}");
			vectors.Add(tilesToPlayer[0].position - previousPosition);
			previousPosition = previousPosition + vectors[i];
			Log.Information($"Vector {vectors[i]} will lead to it");
			tilesToPlayer.RemoveAt(0);
			if (distance <= action.range || tilesToPlayer.Count == 0)
			    break;
		    }
		}
		
	    }
	    Log.Information($"Finished planning the move");
	    return (vectors, action);
	}

	public List<Item> GetLoot()
	{
	    var result = new List<Item>();
	    foreach(Item item in inventory.items.Values)
	    {
		int x = Roller.Roll(100);
		if (x < lootChance * 100)
		{
		    result.Add(item);
		}
	    }
	    return result;
	}
    }
}
