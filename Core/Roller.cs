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
		result += random.Next(dice) + 1;
	    }
	    return result;
	}

	public static int Roll(int dice)
	{
	    return Roll(dice, 1);
	}

	public static int RollWithAdvantage(int dice)
	{
	    return Math.Max(Roll(dice), Roll(dice));
	}

	public static int RollWithDisadvantage(int dice)
	{
	    return Math.Min(Roll(dice), Roll(dice));
	}

	public static int RollWithModification(int dice, int modification)
	{
	    int roll = Roll(dice);
	    if (modification == -1)
		return Math.Min(Roll(dice), roll);
	    else if (modification == 1)
		return Math.Max(Roll(dice), roll);
	    return roll;
	}
    }
}
