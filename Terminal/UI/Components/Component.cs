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
	protected List<object> Properties;

	public Point RelativePosition { get => Parent is not null ? Parent.RelativePosition + Position : Position; }	
	

	// Component properties
	public ComponentProperty<Point> Position { get; }
    public InheritableComponentProperty<ColorObject> Color { get; }



    public Component(Component? parent, Point? position = null)
	{
		Parent = parent;
		Childs = [];
		Properties = [];

		Position = position ?? Point.None;

		Color = new (ColorValue.Reset, Parent?.Color);

		Properties.AddRange([ Position, Color ]);

		if (Parent is null)
			return;

		if (!Parent.Childs.Contains(this))
			Parent.Childs.Add(this);
	}


	public T? As<T>() where T : Component => this as T;


    protected List<T?> PropertiesAs<T>() where T : class
		=> (from property in Properties select property as T).ToList();
	

	public void SetAllPropertiesInherit(bool inherit)
	{
		foreach (IInheritable? property in PropertiesAs<IInheritable>())
			if (property is not null)
				property.Inherit = inherit;
	}


	public void PropertiesCanBeInherited(bool can)
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
		
		foreach (Component child in Childs)
			child.Update();
	}
}
