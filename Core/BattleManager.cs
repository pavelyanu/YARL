using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using Serilog;
using YARL.Actors;
using YARL.Actions;
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
	    StartTurn();
	}

	public void StartTurn()
	{
	    movement_left = player.movement;
	    action_left = 1;
	}

	public void ProcessInput(char key)
	{
	    if (targeting)
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
			    targets.Add(GetEntityAt(cursor));
			    if (targets.Count == chosen_action.numOfTargets)
			    {
				ProcessAction(player, chosen_action, targets);
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
	    } else
	    {
		Vector2 dir;
		switch (key)
		{
		    case 'j':
			dir = new Vector2(0, 1);
			ProcessMovement(player, dir);
			break;
		    case 'k':
			dir = new Vector2(0, -1);
			ProcessMovement(player, dir);
			break;
		    case 'h':
			dir = new Vector2(-1, 0);
			ProcessMovement(player, dir);
			break;
		    case 'l':
			dir = new Vector2(1, 0);
			ProcessMovement(player, dir);
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
			StartTurn();
			break;
		}
	    }
	}

	public void ProcessMovement(Player player, Vector2 dir)
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

	public void MoveCursor(Vector2 dir)
	{
	    var result = cursor + dir;
	    cursor = result;
	}

	public void ProcessAction(Player player, Action action, List<Entity> targets)
	{
	    action.Do(targets);
	    foreach (var target in targets)
	    {
		if (!target.alive)
		{
		    level.RemoveEntity(target);
		}
	    }
	    action_left -= 1;
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
	    sb.Clear();
	    int count = 1;
	    foreach(var action in player.actions.Keys)
	    {
		sb.AppendLine($"{count++} - {action}");
	    }
	    return sb.ToString();
	}

	public string DrawOnBottom()
	{
	    sb.Clear();	
	    sb.AppendLine($"movement - {movement_left}, actions - {action_left}, target mode - {targeting}");
	    if (targeting)
	    {
		var distance = (cursor - player.position).Length();
		sb.AppendLine($"the distance to target is {distance}");
	    }
	    return sb.ToString();
	}
    }
}
