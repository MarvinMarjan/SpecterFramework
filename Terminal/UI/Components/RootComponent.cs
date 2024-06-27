using System;


namespace Specter.Terminal.UI.Components;


/// <summary>
/// The root component of a UI app.
/// All other components should inherit direct
/// or indirectly from this Component.
/// </summary>
public class RootComponent : SectionComponent
{
    public RootComponent() : base(null, drawBorder: false) => SetPropertiesCanBeInherited(false);


    public override void Update()
	{
		base.Update();

		Size.Value = new((uint)Console.LargestWindowWidth, (uint)Console.LargestWindowHeight);
	}
}
