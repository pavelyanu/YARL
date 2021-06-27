using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System;
using Serilog;
using YARL.Actors;
using Action = YARL.Actions.Action;
using YARL.Topography;

namespace YARL.Core
{
    class BattleManager
    {
	Level level;
	Rectangle room;
	Player player;
	public List<Monster> monsters;	
	public Vector2 cursor;	
	List<Entity> initiative;
	int movement_left;
	int action_left;
	public bool targeting;
	Action chosen_action;
	List<Entity> targets;
	StringBuilder sb;
	string bottomMessage;
	string sideMessage;
	bool endingTurn;

	public BattleManager(Level _level, Player _player)
	{
	    level = _level;
	    player = _player;
	    monsters = level.GetMonstersNearPlayer();
	    room = level.currentRoom;
	    cursor = new Vector2(player.position.X, player.position.Y);
	    initiative = new List<Entity>();
	    initiative.AddRange(monsters);
	    initiative.Add(player);
	    sb = new StringBuilder();
	    targeting = false;
	    endingTurn = false;
	    StartTurn();
	}

	public void StartTurn()
	{
	    movement_left = player.movement;
	    action_left = 1;
	}

	public void EndTurn()
	{
	    foreach(var monster in monsters)
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
		    bottomMessage = "";
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
		if ("jkhl1234567890\r".Contains(key))
		{
		    bottomMessage = "";
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
			case '\r':
			    if (!endingTurn)
			    {
				bottomMessage = "This will end your turn. Are you sure?";
				endingTurn = true;
			    } else {
				EndTurn();
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
		if (!Occupied(result))
		{
		    if (level.Move(player, dir))
		    {
			movement_left -= 1;
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
	    sb.Clear();
	    sb.Append(action.Do(targets, player));
	    foreach (var target in targets)
	    {
		if (!target.alive && target is Monster)
		{
		    var monster = target as Monster;
		    var loot = monster.GetLoot();
		    if (loot.Count != 0)
		    {
			foreach(var item in loot)
			{
			    level.PutItem(item, target.position);
			}
		    }
		    level.RemoveEntity(target);
		}
	    }
	    action_left -= 1;
	    bottomMessage = sb.ToString(); 
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
	    int distance = level.GetDistance(player.position, monster.position) - 1;
	    Log.Information($"The between player and monster is {distance}");
	    if (distance <= action.range && action is not null)
	    {
		Log.Information("The monster is close enough for a strike");
		var targets = new List<Entity>();
		targets.Add(player);
		bottomMessage = action.Do(targets, monster);
	    } else Log.Information("The monster is not close enough for a strice");
	    Log.Information("Finished Processing the action");
	    if (!player.alive)
	    {
		sideMessage = "You have died";
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
	    
	public string DrawOnSide()
	{
	    if (sideMessage is null || sideMessage == "")
	    {
		sb.Clear();
		int count = 1;
		foreach(var action in player.actions.Keys)
		{
		    sb.AppendLine($"{count++} - {action}");
		}
		return sb.ToString();
	    } else
	    {
		return sideMessage;
	    }
	}

	public string DrawOnBottom()
	{
	    if (bottomMessage is null || bottomMessage == "")
	    {
		sb.Clear();	
		sb.AppendLine($"movement - {movement_left}, actions - {action_left}, target mode - {targeting}");
		sb.AppendLine($"Your current health is: {player.health}");
		if (targeting)
		{
		    var distance = (cursor - player.position).Length();
		    sb.AppendLine($"the distance to target is {distance}");
		}
		return sb.ToString();
	    } else
	    {
		return bottomMessage;
	    }
	}
    }
}
