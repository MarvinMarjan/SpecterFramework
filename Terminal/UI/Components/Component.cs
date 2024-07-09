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


/// <summary>
/// Classes that inherit from this can't be parent of any component.
/// </summary>
public interface IChildLess {}



/// <summary>
/// Base class of all UI components.
/// </summary>
public abstract partial class Component : IUpdateable, IDrawable
{

	// Properties

	public Component? Parent { get; set; }
	public List<Component> Childs { get; private set; }

	public ComponentPropertiesManager PropertiesManager { get; protected set; }
	
	public Bounds Bounds => Bounds.FromRectangle(Position, Size);


	/// <summary>
	/// The position of this Component relative to its parent, if not null.
	/// </summary>
	public Point RelativePosition => Parent is not null ? Parent.RelativePosition + Position : Position;


	/// <summary>
	/// The Rect object of this Component.
	/// </summary>
	public Rect Rect => new(Position, Size);
	

	// Component properties

	public ComponentProperty<Point> Position { get; }
	public ComponentProperty<Size> Size { get; }
	public InheritableComponentProperty<Alignment> Alignment { get; }
	public InheritableComponentProperty<ColorObject> Color { get; }



	public delegate void UpdateEventHandler();

	public event UpdateEventHandler? UpdateEvent;

	protected void RaiseUpdateEvent() => UpdateEvent?.Invoke();



	public Component(

		Component? parent,
		Point? position = null,
		Size?  size     = null,

		// * If alignment != Alignment.None (and null), Position is ignored and Alignment is used to define the
		// * Component position
		Alignment? alignment = null,
		
		ColorObject? color = null,

		bool inheritProperties = true


	)
	{
		Parent = parent;
		ChildLessParentCheck(); // checks if Parent is a IChildLess

		Childs        = [];
		PropertiesManager = new(this, new(inheritProperties, true));


		Position = new(
			this, "Position", position ?? Point.None,
			true, true, true
		);
		
		Size = new(
			this, "Size", size ?? UI.Size.None,
			true, true, true
		);

		Alignment = new(
			this, "Alignment", alignment ?? UI.Alignment.None,
			Parent?.Alignment, requestRenderOnChange: true, drawAllRequest: true
		);

		Color = new(
			this, "Color", color ?? ColorObject.None,
			Parent?.Color, requestRenderOnChange: true
		);


		PropertiesManager.GetAllPropertiesAs<IInheritable>().SetInheritablesInherit(inheritProperties);


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



	public void AddThisToRenderQueue()
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
		foreach (IUpdateable? property in PropertiesManager.GetAllPropertiesAs<IUpdateable>())
			property?.Update();
		
		Position.DefaultValue = Alignment.Value.CalculatePosition(this);

		RaiseUpdateEvent();
		
		foreach (Component child in Childs)
			child.Update();
	}
}
