namespace Specter.Terminal.UI.Components.Property;


public class ComponentPropertyAttributes(
	bool updateOnChange = false, bool requestOwnerRenderOnPropertyChange = false, bool drawAllRequest = false,
	bool ignoreManagerRequirement = false
)
{
	/// <summary>
	/// Should DefaultValue be setted immediately to Value when
	/// it changes?
	/// </summary>
	public bool UpdateOnChange { get; set; } = updateOnChange;

	public bool RequestOwnerRenderOnPropertyChange { get; set; } = requestOwnerRenderOnPropertyChange;
	public bool DrawAllRequest { get; set; } = drawAllRequest;
	
	public bool IgnoreManagerRequirement { get; set; } = ignoreManagerRequirement;
}


public class InheritableComponentPropertyAttributes(
	bool updateOnChange = false, bool requestOwnerRenderOnPropertyChange = false, bool drawAllRequest = false,
	bool ignoreManagerRequirement = false, bool inherit = true, bool canBeInherited = true

) : ComponentPropertyAttributes(
	updateOnChange, requestOwnerRenderOnPropertyChange, drawAllRequest, ignoreManagerRequirement
)
{
	public bool Inherit { get; set; } = inherit;
	public bool CanBeInherited { get; set; } = canBeInherited;
}