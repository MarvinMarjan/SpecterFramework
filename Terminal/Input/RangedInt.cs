namespace Specter.Terminal.Input;


public struct RangedInt
{
	private int _value;
	public int Value
	{
		readonly get => _value;
		set
		{
			if (IsValueInsideRange(value))
				_value = value;
		}
	}

	public int Min { get; set; }
	public int Max { get; set; }



	public RangedInt(int value, int min, int max)
	{
		Min = min;
		Max = max;

		if (IsValueInsideRange(value))
			Value = value;
		else
			Value = Min;
	}



	public readonly bool IsValueInsideRange(int value)
		=> value >= Min && value <= Max;


	public readonly bool IsAtStart()
		=> Value == Min;

	public readonly bool IsAtEnd()
		=> Value == Max;



	public static RangedInt operator++(RangedInt ranged)
	{
		ranged.Value++;
		return ranged;
	}

	public static RangedInt operator--(RangedInt ranged)
	{
		ranged.Value--;
		return ranged;
	}


	public static int operator+(RangedInt left, RangedInt right)
		=> left.Value + right.Value;

	public static int operator-(RangedInt left, RangedInt right)
		=> left.Value - right.Value;

	public static int operator*(RangedInt left, RangedInt right)
		=> left.Value * right.Value;
	
	public static int operator/(RangedInt left, RangedInt right)
		=> left.Value / right.Value;


	public static implicit operator int(RangedInt ranged) => ranged.Value;
}