using System.Collections.Generic;
using Serilog;

namespace YARL.Core
{
    public class GameLog
    {
	List<string> log;
	public List<string> view { get; protected set; }
	public int size { get; protected set; }
	public bool showinglast { get; protected set; }

	public GameLog(int _size)
	{
	    size = _size;
	    view = new List<string>();
	    showinglast = true;
	    log = new List<string>();
	}
	
	public void SetViewToLastN()
	{
	    view.Clear();
	    showinglast = true;
	    int lacking = size - log.Count;
	    if (lacking <= 0)
	    {
		for(int i = log.Count - size; i < log.Count; i++)
		{
		    view.Add(log[i]);
		}
	    } else 
	    {
		view.AddRange(log);
		for(int i = 0; i < size - lacking - 1; i++)
		{
		    view.Add("");
		}
	    }
	}

	public void Add(string entry)
	{
	    log.Add(entry);
	    SetViewToLastN();
	}

    }
}
