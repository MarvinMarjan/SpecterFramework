using System.Collections.Generic;
using System.Text;
using Specter.Color;

namespace Specter.Terminal.UI.Components;


// Base class of all UI components
public abstract class Component
{
	public Component? Parent { get; set; }
	public List<Component> Childs { get; set; } = [];

	public Point Position { get; set; }
	public Point RelativePosition { get => Parent is not null ? Parent.RelativePosition + Position : Position; }

	public ColorObject Color { get; set; } = ColorObject.Default;


	public Component(Component? parent, Point? position = null)
	{
		Parent = parent;
		Position = position ?? Point.None;
	
		if (Parent is null)
			return;

		Color = Parent.Color;

		if (!Parent.Childs.Contains(this))
			Parent.Childs.Add(this);
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
		foreach (Component child in Childs)
			child.Update();
	}
}
