using System; 

namespace YARL.Core
{
    public static class Roller
    {
	static Random random;

	public static int Roll(int dice, int n)
	{
	    random = new Random();
	    int result = 0;
	    for (int i = 0; i < n; i++)
	    {
		result += random.Next(1, dice);
	    }
	    return result;
	}

	public static int Roll(int dice)
	{
	    return Roll(dice, 1);
	}
    }
}
