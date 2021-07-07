using System.Collections.Generic;
using YARL.Items;
using YARL.Actors;

namespace YARL.Core
{
    class CharacterManager
    {
	public bool creating { get; protected set; }
	public bool leveling { get; protected set; }
	public InventoryManager inventoryManager { get; protected set; }
	public Player player;
	public GameLog gameLog;
	public int selectedLine { get; protected set; }
	Dictionary<string, int> chooseMap;

	public CharacterManager(GameLog _gameLog, Player _player)
	{
	    gameLog = _gameLog;
	    creating = true;
	    leveling = false;
	    selectedLine = 0;
	    player = _player;
	    player.leveledUp += LeveledUp;
	}

	void LeveledUp(int level)
	{
	    leveling = true;
	}

	public void ProcessInput(char key)
	{
	    if (creating)
	    {
		if (key == 'j')
		{
		    selectedLine = (selectedLine + 1) % 2;
		} else if (key == 'k')
		{
		    selectedLine = (selectedLine - 1) % 2;
		    if (selectedLine < 0)
			selectedLine *= -1;
		} else if (key == 'h'
	    }
	}

	public List<string> DrawCreationMenu()
	{
	    
	}

	public List<string> DrawLevelingMenu()
	{

	}

	Dictionary<string, int> GetChooseMap()
	{
	    var result = new Dictionary<string, int>();
	    result["str"] = player.str;
	    result["dex"] = player.dex;
	    return result;
	}

	void SetStats()
	{
	    player.SetStr(chooseMap["str"]);
	    player.SetDex(chooseMap["dex"]);
	}
    }
}
