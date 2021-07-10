using System.Collections.Generic;
using System.Linq;
using YARL.Actors;

namespace YARL.Core
{
    class CharacterManager
    {
	public bool leveling { get; protected set; }
	public InventoryManager inventoryManager { get; protected set; }
	public Player player;
	public GameLog gameLog;
	public int selectedLine { get; protected set; }
	public int points { get; protected set; }
	Dictionary<string, int> chooseMap;
	Dictionary<string, int> originalMap;

	public CharacterManager(Player _player, GameLog _gameLog ) 
	{
	    gameLog = _gameLog;
	    leveling = true;
	    points = 2;
	    selectedLine = 0;
	    player = _player;
	    player.leveledUp += LeveledUp;
	    SetMaps();
	}

	void LeveledUp(int level)
	{
	    gameLog.Add($"Player has reached level {level}");
	    leveling = true;
	    points = 1;
	    SetMaps();
	}

	public void ProcessInput(char key)
	{
	    if (leveling)
	    {
		if (key == 'j')
		{
		    selectedLine = (selectedLine + 1) % 2;
		} else if (key == 'k')
		{
		    selectedLine = (selectedLine - 1) % 2;
		    if (selectedLine < 0)
			selectedLine *= -1;
		} else if (key == 'l')
		{
		    IncrementStat();
		} else if (key == 'h')
		{
		    DecrementStat();    
		} else if (key == '\n')
		{
		    if (points == 0)
		    {
			player.SetStats(chooseMap);
			leveling = false;
		    }
		}
	    }
	}

	public List<string> Draw()
	{
	    var result = new List<string>();
	    foreach(var pair in chooseMap)
	    {
		result.Add($"{pair.Key}: {pair.Value}");
	    }
	    result.Add($"Points left: {points}");
	    return result;
	}

	void SetMaps()
	{
	    chooseMap = player.GetStats();
	    originalMap = player.GetStats();
	}

	string GetCurrentChooseKey()
	{
	    return chooseMap.Keys.ToArray()[selectedLine];
	}

	bool IncrementStat()
	{
	    if (points > 0)
	    {
		points--;
		chooseMap[GetCurrentChooseKey()]++;
		return true;
	    }
	    return false;
	}

	bool DecrementStat()
	{
	    if (chooseMap[GetCurrentChooseKey()]  > originalMap[GetCurrentChooseKey()])
	    {
		points++;
		chooseMap[GetCurrentChooseKey()]--;
		return true;
	    }
	    return false;
	}
    }
}
