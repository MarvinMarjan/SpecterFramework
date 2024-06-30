using System;
using System.Collections.Generic;
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
	protected List<object> AllProperties;
	protected object[] Properties => [ Position, Size, Alignment, Color ];


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

		Childs        = [];
		AllProperties = [];

		Position = new(position ?? Point.None, true);
		Size     = new(size ?? UI.Size.None, true);

		Alignment = new(alignment ?? UI.Alignment.None, Parent?.Alignment);

		Color = new(color ?? ColorObject.None, Parent?.Color);


		// * keep synchronized
		// TODO: check if AllProperties already containts Properties
		AllProperties.AddRange(Properties);

		// TODO: try to store properties automatically instead of doing it manually.

		Properties.PropertiesAs<IInheritable>().SetInheritablesInherit(inheritProperties);
		RequestRenderOnPropertiesChange(Properties.PropertiesAs<IComponentPropertyEvents>());

		if (Parent is null)
			return;

		// add this component as child of parent
		if (!Parent.Childs.Contains(this))
			Parent.Childs.Add(this);
	}


	public bool IsChildOf(Component component)
	{
		if (component.Childs.Count == 0)
			return false;

		if (component.Childs.Contains(this))
			return true;
		
		foreach (Component child in component.Childs)
			if (IsChildOf(child))
				return true;

		return false;
	}


	public void ChildLessParentCheck()
	{
		if (Parent is IChildLess)
			throw new Exception("Can't have a 'IChildLess' as parent.");
	}



	/// <typeparam name="T"> The Component derived type to convert. </typeparam>
	/// <returns> This Component converted to the specified Component derived type. </returns>
	public T? As<T>() where T : Component => this as T;



	protected T[] PropertiesAs<T>() where T : class
		=> Properties.PropertiesAs<T>();

	protected T[] AllPropertiesAs<T>() where T : class
		=> AllProperties.ToArray().PropertiesAs<T>();


	protected void SetPropertiesInherit(bool inherit)
		=> PropertiesAs<IInheritable>().SetInheritablesInherit(inherit);

	protected void SetPropertiesCanBeInherited(bool can)
		=> PropertiesAs<IInheritable>().SetInheritablesCanBeInherited(can);

	protected void SetAllPropertiesCanBeInherited(bool can)
		=> AllPropertiesAs<IInheritable>().SetInheritablesCanBeInherited(can);



	protected void RequestRenderOnPropertiesChange(IComponentPropertyEvents[] properties)
	{
		foreach (var property in properties)
			RequestRenderOnPropertyChange(property);
	}

	protected void RequestRenderOnPropertyChange(IComponentPropertyEvents property)
		=> property.PropertyValueChanged += delegate { AddThisToRenderQueue(); };

	protected void AddThisToRenderQueue()
	{
		// * do not add if there is already a parent in the queue, since
		// * drawing the parent also draws the child.

		foreach (Component component in App.RenderQueue)
			if (IsChildOf(component))
				return;

		App.AddComponentToRenderQueue(this);
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
		foreach (IUpdateable? property in AllPropertiesAs<IUpdateable>())
			property?.Update();
		
		Position.DefaultValue = Alignment.Value.CalculatePosition(this);
		
		foreach (Component child in Childs)
			child.Update();
	}
}
