using System;
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


public interface IChildLess {}



/// <summary>
/// Base class of all UI components.
/// </summary>
public abstract partial class Component : IUpdateable, IDrawable
{

	// Properties

	public Component? Parent { get; set; }
	public List<Component> Childs { get; set; }
	
	public Bounds Bounds { get => Bounds.FromRectangle(Position, Size); }
	

	/// <summary>
	/// List of all component properties
	/// </summary>
	protected List<object> Properties;


	/// <summary>
	/// The position of this Component relative to its parent, if not null.
	/// </summary>
	public Point RelativePosition { get => Parent is not null ? Parent.RelativePosition + Position : Position; }


	/// <summary>
	/// The Rect object of this Component.
	/// </summary>
	public Rect Rect { get => new(Position, Size); }
	

	// Component properties

	public ComponentProperty<Point> Position { get; }
	public ComponentProperty<Size> Size { get; }
	public InheritableComponentProperty<Alignment> Alignment { get; }
	public InheritableComponentProperty<ColorObject> Color { get; }



	public Component(

		Component? parent,
		Point? position = null,
		Size?  size     = null,

		// If alignment != Alignment.None, Position is ignored and Alignment is used to define the
		// Component position
		Alignment? alignment = null,
		
		ColorObject? color = null,

		bool inheritProperties = true
	)
	{
		Parent = parent;
		ChildLessParentCheck(); // checks if Parent is a IChildLess

		Childs     = [];
		Properties = [];

		Position = position ?? Point.None;
		Size = size ?? UI.Size.None;

		Alignment = new(
			alignment ?? UI.Alignment.None, Parent?.Alignment, false // * temporary. remove later
		);

		Color = new(color ?? ColorValue.Reset, Parent?.Color);

		Properties.AddRange([ Position, Size, Color ]);

		SetAllPropertiesInherit(inheritProperties);

		if (Parent is null)
			return;

		// add this component as child of parent
		if (!Parent.Childs.Contains(this))
			Parent.Childs.Add(this);
	}



	public void ChildLessParentCheck()
	{
		if (Parent is IChildLess)
			throw new Exception("Can't have a 'IChildLess' as parent.");
	}



	/// <typeparam name="T"> The Component derived type to convert. </typeparam>
	/// <returns> This Component converted to the specified Component derived type. </returns>
	public T? As<T>() where T : Component => this as T;

	
	/// <typeparam name="T"> The type to convert. </typeparam>
	/// <returns> All component properties converted to another type, or null, if not possible. </returns>
	protected List<T?> PropertiesAs<T>() where T : class
		=> (from property in Properties select property as T).ToList();
		

	/// <summary>
	/// Defines whether all component properties can inherit from its parents or not>
	/// </summary>
	/// <param name="inherit"> The value. </param>
	public void SetAllPropertiesInherit(bool inherit)
	{
		foreach (var property in PropertiesAs<IInheritable>())
			if (property is not null)
				property.Inherit = inherit;
	}


	/// <summary>
	/// Defines whether all component properties can be inherited by its child or not
	/// </summary>
	/// <param name="can"> The value. </param>
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
