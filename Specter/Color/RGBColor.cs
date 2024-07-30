namespace Specter.Color;


/// <summary>
/// Encapsulates a RGB structure.
/// </summary>
/// <param name="r"> The red channel. </param>
/// <param name="g"> The green channel. </param>
/// <param name="b"> The blue channel. </param>
public struct ColorRGB(byte? r = null, byte? g = null, byte? b = null)
{
	public byte? Red { get; set; } = r;
	public byte? Green { get; set; } = g;
	public byte? Blue { get; set; } = b;

	
	public ColorRGB(byte? all) : this(all, all, all)
	{}


	public readonly bool AreAllChannelsNull()
		=> Red is null && Green is null && Blue is null;

	public void SetValueToNullChannels(byte value)
	{
		Red ??= value;
		Green ??= value;
		Blue ??= value;
	}

	public void SetAll(byte? value) => Red = Green = Blue = value;
}
