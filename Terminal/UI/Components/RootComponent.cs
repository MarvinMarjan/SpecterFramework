namespace Specter.Terminal.UI.Components;


/// <summary>
/// The root component of a UI app.
/// All other components should inherit direct
/// or indirectly from this Component.
/// </summary>
public class RootComponent : SectionComponent
{
	public RootComponent() : base("Root",null, drawBorder: false)
	{
		PropertiesManager.Requirement.CanBeInherited = false;
	}


	public override void Update()
	{
		// * setting the Size before updating the base (consequently, updating the childs)
		// * is needed to avoid drawing issues when the terminal gets resized.

		if (TerminalAttributes.TerminalResized)
			Size.DefaultValue = TerminalAttributes.TerminalSize;
		
		base.Update();
	}
}
