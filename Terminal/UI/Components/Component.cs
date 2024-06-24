using System.Collections.Generic;
using System.Linq;
using System.Text;

using Specter.Color;


namespace Specter.Terminal.UI.Components;


public interface IUpdateable
{
	public void Update();
}

public interface IDrawable
{
	public string Draw();
}


// Base class of all UI components
public abstract class Component : IUpdateable, IDrawable
{
	// Properties

	public Component? Parent { get; set; }
	public List<Component> Childs { get; set; }
	protected List<object> Properties; // List of all component properties

	public Point RelativePosition { get => Parent is not null ? Parent.RelativePosition + Position : Position; }
	
	public Rect Rect { get => new(Position, Size); }
	

	// Component properties
	public ComponentProperty<Point> Position { get; }
	public ComponentProperty<Size> Size { get; }
	public InheritableComponentProperty<Alignment> Alignment { get; }
	public InheritableComponentProperty<ColorObject> Color { get; }



	// TODO: add an interface that blocks a Component from having childs (Components can't set it as their parent).
	// i.e.: public class TextComponent : Component, IChildLess

	public Component(

		Component? parent,
		Point? position = null,
		Size?  size     = null,

		Alignment? alignment = null,

		ColorObject? color = null

		// TODO: add a parameter that defines if the Component should inherit its parent properties
	)
	{
		Parent     = parent;
		Childs     = [];
		Properties = [];

		Position = position ?? Point.None;
		Size = size ?? UI.Size.None;

		Alignment = new(
			alignment ?? UI.Alignment.None, Parent?.Alignment, false // * temporary. remove later
		);

		Color = new(color ?? ColorValue.Reset, Parent?.Color);

		Properties.AddRange([ Position, Size, Color ]);

		if (Parent is null)
			return;

		// add this component as child of parent
		if (!Parent.Childs.Contains(this))
			Parent.Childs.Add(this);
	}



	// Casts this component to another one
	public T? As<T>() where T : Component => this as T;

	
	// Casts all component properties to another type, if possible
	protected List<T?> PropertiesAs<T>() where T : class
		=> (from property in Properties select property as T).ToList();
		

	// Defines whether all component properties can inherit from its parents or not>
	public void SetAllPropertiesInherit(bool inherit)
	{
		foreach (var property in PropertiesAs<IInheritable>())
			if (property is not null)
				property.Inherit = inherit;
	}


	// Defines whether all component properties can be inherited by its child or not
	public void SetPropertiesCanBeInherited(bool can)
	{
		foreach (IInheritable? property in PropertiesAs<IInheritable>())
			if (property is not null)
				property.CanBeInherited = can;
	}



	public virtual string Draw()
	{
		StringBuilder builder = new();

		foreach (Component child in Childs)
			builder.Append(child.Draw());

		return builder.ToString();
	}

	public virtual void Update()
	{
		foreach (IUpdateable? property in PropertiesAs<IUpdateable>())
			property?.Update();

		Position.Value = Alignment.Value.CalculatePosition(this);
		
		foreach (Component child in Childs)
			child.Update();
	}
}
