namespace Specter.Util;


public static class Bit
{
	public static int Or(int[] values)
		=> values.OperateAll(
			0, (ref int accumulator, int value) => accumulator |= value
		);
	

	public static int And(int[] values)
		=> values.OperateAll(
			0, (ref int accumulator, int value) => accumulator &= value
		);

	
	public static bool HasFlag(int flags, int flag)
		=> (flags & flag) == flag;
}