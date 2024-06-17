using System.Runtime.CompilerServices;

namespace Specter.Color;


public struct RGBColor(byte? r = null, byte? g = null, byte? b = null)
{
	public byte? r = r;
	public byte? g = g;
	public byte? b = b;


	public readonly bool AreAllChannelsNull()
	{
		return r is null && g is null && b is null;
	}

	public void SetValueToNullChannels(byte value)
	{
		r ??= value;
		g ??= value;
		b ??= value;
	}

	public void SetAll(byte? value)
	{
		r = g = b = value;
	}
}
