using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Serilog;
using YARL.Actors;
using Action = YARL.Actions.Action;
using YARL.Topography;

namespace YARL.Core
{
    public class BattleManager
    {
	Level level;
	Rectangle room;
	Player player;
	GameLog gameLog;
	public Vector2 cursor;	
	List<Entity> initiative;
	int movement_left;
	int action_left;
	public bool targeting;
	Action chosen_action;
	List<Entity> targets;
	bool endingTurn;

	public BattleManager(Level _level, Player _player, GameLog _gameLog)
	{
	    level = _level;
	    player = _player;
	    gameLog = _gameLog;
	    room = level.currentRoom;
	    cursor = new Vector2(player.position.X, player.position.Y);
	    initiative = new List<Entity>();
	    initiative.AddRange(level.GetMonstersNearPlayer());
	    initiative.Add(player);
	    targeting = false;
	    endingTurn = false;
	    if (Roller.Roll(3) == 1)
	    {
		EndTurn();
	    } else {
		StartTurn();
	    }
	}

	public void StartTurn()
	{
	    movement_left = player.movement;
	    action_left = player.n_of_actions;
	}

	public void EndTurn()
	{
	    foreach(var monster in GetMonsters())
	    {
		Log.Information($"Logging actions of the {monster.name}");
		ProcessMonsterAction(monster);
	    }
	    StartTurn();
	}

	public void ProcessInput(char key)
	{
	    if (targeting)
	    {
		if ("jkhl\n\r".Contains(key))
		{
		    Vector2 dir = new Vector2(0, 0);
		    switch (key)
		    {
			case 'j':
			    dir = new Vector2(0, 1);
			    break;
			case 'k':
			    dir = new Vector2(0, -1);
			    break;
			case 'h':
			    dir = new Vector2(-1, 0);
			    break;
			case 'l':
			    dir = new Vector2(1, 0);
			    break;
			case '\n':
			    if (chosen_action is not null && GetEntityAt(cursor) is not null)
			    {
				if (GetEntityAt(cursor) is not Player)
				{
				    targets.Add(GetEntityAt(cursor));
				}
				if (targets.Count == chosen_action.numOfTargets)
				{
				    ProcessPlayerAction(player, chosen_action, targets);
				    cursor = player.position;
				    targeting = false;
				}
			    }
			    break;
			case '\r':
			    targeting = false;
			    break;
			
		    }
		    var result = cursor + dir;
		    var difference = player.position - result;
		    float distance = difference.Length();
		    Log.Information("Trying to move the cursor");
		    Log.Information($"The distance is {distance}");
		    if (distance < chosen_action.range + 1)
		    {
			MoveCursor(dir);
		    }
		}
	    } else
	    {
		if ("jkhlny1234567890 \r".Contains(key))
		{
		    Vector2 dir;
		    switch (key)
		    {
			case 'j':
			    dir = new Vector2(0, 1);
			    ProcessPlayerMovement(player, dir);
			    break;
			case 'k':
			    dir = new Vector2(0, -1);
			    ProcessPlayerMovement(player, dir);
			    break;
			case 'h':
			    dir = new Vector2(-1, 0);
			    ProcessPlayerMovement(player, dir);
			    break;
			case 'l':
			    dir = new Vector2(1, 0);
			    ProcessPlayerMovement(player, dir);
			    break;
			case '1':
			    Log.Information("Battlemanager has recorded 1");
			    if (player.actions.Keys.Count > 0 && action_left > 0)
			    {
				Log.Information("Player may make an aciton");
				chosen_action = player.actions.Values.ToList()[0];
				targeting = true;
				cursor = new Vector2(player.position.X, player.position.Y);
				targets = new List<Entity>();
			    }else
			    {
				Log.Information("Player may not make an aciton");
			    }
			    break;
			case ' ':
			    if (!endingTurn)
			    {
				gameLog.Add("This will end your turn. Are you sure? Enter y/n.");
				endingTurn = true;
			    } else {
				gameLog.Add("This will end your turn. Are you sure? Enter y/n.");
			    }
			    break;
			case 'y':
			    if (endingTurn)
			    {
				EndTurn();
				endingTurn = false;
			    }
			    break;
			case 'n':
			    if (endingTurn)
			    {
				endingTurn = false;
			    }
			    break;
		    }
		}
	    }
	}

	public void ProcessPlayerMovement(Player player, Vector2 dir)
	{
	    if (movement_left != 0)
	    {
		var result = player.position + dir;
		var previous = player.position;
		if (!Occupied(result))
		{
		    if (level.Move(player, dir))
		    {
			foreach(var monster in GetMonsters())
			{
			    if (level.GetDistance(result, monster.position) == 2 &&
				level.GetDistance(previous, monster.position) == 1)
			    {
				gameLog.Add($"{monster.name} tries to hit you as you run");
				var action = monster.MakeMove(level).Item2;
				if (action.range == 1)
				{
				    var targets = new List<Entity>();
				    targets.Add(player);
				    gameLog.Add($"{action.Do(targets, monster, level)}");
				}
			    }
			}
			movement_left -= 1;
			if (!player.alive)
			{
			    gameLog.Add("Player has died!");
			}
		    }
		}
	    }
	}

	public void ProcessMonsterMovement(Monster monster, Vector2 dir)
	{
	    Log.Information($"Started processing move by {dir} by {monster}");
	    var result = monster.position + dir;
	    if (!Occupied(result) && room.Contains(result))
	    {
		Log.Information("The move is legal");
		level.Move(monster, dir);
	    } else Log.Information("The move is illegal");
	    Log.Information($"Finished processing the move {dir} by {monster}");
	}

	public void MoveCursor(Vector2 dir)
	{
	    var result = cursor + dir;
	    cursor = result;
	}

	public void ProcessPlayerAction(Player player, Action action, List<Entity> targets)
	{
	    gameLog.Add(action.Do(targets, player, level));
	    foreach (var target in targets)
	    {
		Log.Information($"{target.name}'s health is {target.health}");
		if (!target.alive && target is Monster)
		{
		    Log.Information($"{target.name} is dead");
		    gameLog.Add($"You have killed {target.name}");
		    var monster = target as Monster;
		    var loot = monster.GetLoot();
		    if (loot.Count != 0)
		    {
			foreach(var item in loot)
			{
			    level.PutItem(item, target.position);
			}
		    }
		    player.AddExp(monster.exp);
		    gameLog.Add($"Player has earned {monster.exp} experience");
		    level.RemoveEntity(target);
		    initiative.Remove(target);
		}
	    }
	    action_left -= 1;
	}

	public void ProcessMonsterAction(Monster monster)
	{
	    var tuple = monster.MakeMove(level);
	    var vectors = tuple.Item1;
	    var action = tuple.Item2;
	    Log.Information($"there are {vectors.Count} vectors of movement");
	    foreach(var vector in vectors)
	    {
		ProcessMonsterMovement(monster, vector);
	    }
	    Log.Information($"Action to be attempet is {action.name}");
	    int distance = level.GetDistance(player.position, monster.position);
	    Log.Information($"The between player and monster is {distance}");
	    if (distance <= action.range && action is not null)
	    {
		Log.Information("The monster is close enough for a strike");
		var targets = new List<Entity>();
		targets.Add(player);
		gameLog.Add(action.Do(targets, monster, level));
	    } else Log.Information("The monster is not close enough for a strice");
	    Log.Information("Finished Processing the action");
	    if (!player.alive)
	    {
		gameLog.Add("Player has died!");
	    }
	}

	bool Occupied(Vector2 position)
	{
	    foreach (var entity in initiative)
	    {
		if (entity.position == position)
		    return true;
	    }
	    return false;
	}

	Entity GetEntityAt(Vector2 position)
	{
	    foreach (var entity in initiative)
	    {
		if (entity.position == position)
		    return entity;
	    }
	    return null;
	}

	public List<Monster> GetMonsters()
	{
	    var result = new List<Monster>();
	    foreach(var entity in initiative)
	    {
		if (entity is Monster)
		    result.Add(entity as Monster);
	    }
	    return result;
	}
	    
	public List<string> DrawOnSide()
	{
	    var result = new List<string>();
	    int count = 1;
	    foreach(var action in player.actions.Keys)
	    {
		result.Add($"{count++} - {action}");
	    }
	    return result;
	}

	public List<string> DrawPlayerInfo()
	{
	    var result = new List<string>();
	    result.Add($"movement : {movement_left}");
	    result.Add($"actions : {action_left}");
	    result.Add($"target mode ^ {targeting}");
	    if (targeting)
	    {
		var distance = (cursor - player.position).Length();
		result.Add($"the distance to target is {distance}");
	    }
	    return result;
	}
    }
}
