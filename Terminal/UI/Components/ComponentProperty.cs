namespace Specter.Terminal.UI.Components;


// Represents properties of UI components
public class ComponentProperty<T>
{
    private T _value;

    public T Value
	{
		get => _value;
		set
		{
			_value = value;
			RaiseValueChangedEvent(value);
		}
	}


	public ComponentProperty(T value)
	{
		_value = value;
		PropertyValueChanged += OnPropertyValueChange;
	}


	public static implicit operator T(ComponentProperty<T> property) => property.Value;
	public static implicit operator ComponentProperty<T>(T value) => new(value);
	

	public delegate void ValueChangedEventHandler(T newValue);

	public event ValueChangedEventHandler? PropertyValueChanged;

	public void RaiseValueChangedEvent(T newValue) => PropertyValueChanged?.Invoke(newValue);


	public virtual void OnPropertyValueChange(T newValue) {}
}



public interface IInheritable : IUpdateable
{
	public bool Inherit { get; set; }
	public bool ConstantValueUpdate { get; set; }
	public bool CanBeInherited { get; set; }
}


// ComponentProperty that can inherit values from its parent
public class InheritableComponentProperty<T> : ComponentProperty<T>, IInheritable
{
    public InheritableComponentProperty<T>? ParentProperty { get; set; }

	public bool Inherit { get; set; }
	public bool CanBeInherited { get; set; }
	public bool ConstantValueUpdate { get; set; }


	public InheritableComponentProperty(
		T value, InheritableComponentProperty<T>? parent = null, bool inherit = true, bool canBeInherited = true
	) : base(value)
	{
		ParentProperty = parent;
		Inherit = inherit;
		CanBeInherited = canBeInherited;
		ConstantValueUpdate = true;

		if (ParentProperty is null)
			return;

		if (CanInherit())
			Value = ParentProperty.Value;
			
		ParentProperty.PropertyValueChanged += OnParentPropertyValueChange;
	}


	public bool CanInherit() => Inherit && ParentProperty is not null && ParentProperty.CanBeInherited;


	public static implicit operator T(InheritableComponentProperty<T> property) => property.Value;
	public static implicit operator InheritableComponentProperty<T>(T value) => new(value);


	public virtual void OnParentPropertyValueChange(T newValue)
	{
		if (CanInherit())
			Value = newValue;
	}


	public virtual void Update()
	{
		if (ParentProperty is not null && CanInherit() && ConstantValueUpdate)
			Value = ParentProperty.Value;
	}
}