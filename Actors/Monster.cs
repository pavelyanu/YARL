using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using YARL.Actions;
using YARL.Core;
using YARL.Drawing;
using YARL.Items;

namespace YARL.Actors
{
    public class Monster: Entity
    {
	public List<Item> loot { get; set; }
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
	    List<Item> _loot,
	    double _lootchance,
	    Action _action
	    ) : base(_glyph, _drawBehaviour, _movement, _name, _str, _dex, _inte, _health)
	{
	    loot = _loot;
	    lootChance = _lootchance;
	    actions.Add(_action.name, _action);
	}

	public (List<Vector2>, Action) MakeMove(Level level)
	{
	    var vectors = new List<Vector2>();    
	    var action = actions.Values.ToList()[0];
	    var tilesToPlayer = level.GetLine(position, level.GetPlayerPosition());
	    vectors.Add(position - tilesToPlayer[0].position);
	    tilesToPlayer.RemoveAt(0);
	    for(int i = 1; i < movement; i++)
	    {
		vectors.Add(vectors[i - 1] - tilesToPlayer[0].position);
		tilesToPlayer.RemoveAt(0);
	    }
	    return (vectors, action);
	}

	public List<Item> GetLoot()
	{
	    var result = new List<Item>();
	    foreach(var item in loot)
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
