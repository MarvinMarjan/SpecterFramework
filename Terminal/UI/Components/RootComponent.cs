using System;


namespace Specter.Terminal.UI.Components;


public class RootComponent : SectionComponent
{
    public RootComponent() : base(null, drawBorder: false) => SetPropertiesCanBeInherited(false);


    public override void Update()
	{
		base.Update();

		Size.Value = new((uint)Console.LargestWindowWidth, (uint)Console.LargestWindowHeight);
	}
}
