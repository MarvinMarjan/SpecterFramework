namespace Specter.Color;


/// <summary>
/// Encapsulates a RGB structure.
/// </summary>
/// <param name="r"> The red channel. </param>
/// <param name="g"> The green channel. </param>
/// <param name="b"> The blue channel. </param>
public struct ColorRGB(byte? r = null, byte? g = null, byte? b = null)
{
	public byte? r = r;
	public byte? g = g;
	public byte? b = b;

	// TODO: create a constructor that sets a single value to all channels. (SetAll wrapper)

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
