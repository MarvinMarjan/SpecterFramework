namespace Specter.Terminal.UI.Components;


// Represents properties of UI components
public class ComponentProperty<T>
{
    private T _value;

	// TODO: Add default values.
	// That way, if a property is constructed with a initial value, but defined as inherit, the initial value
	// is not lost, allowing it to be used in future


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
	public IInheritable? Parent { get; set; }

	public bool Inherit { get; set; }
	public bool ConstantValueUpdate { get; set; }
	public bool CanBeInherited { get; set; }


	public void InheritValue();
}


// ComponentProperty that can inherit values from its parent
public class InheritableComponentProperty<T> : ComponentProperty<T>, IInheritable
{
    public IInheritable? Parent { get; set; }

	public InheritableComponentProperty<T>? ParentAsProperty
	{
		get => Parent as InheritableComponentProperty<T>;
	}


	public bool Inherit { get; set; }
	public bool CanBeInherited { get; set; }
	public bool ConstantValueUpdate { get; set; }


	public InheritableComponentProperty(
		T value, InheritableComponentProperty<T>? parent = null, bool inherit = true, bool canBeInherited = true
	) : base(value)
	{
		Parent = parent;
		Inherit = inherit;
		CanBeInherited = canBeInherited;
		ConstantValueUpdate = true;

		if (ParentAsProperty is null)
			return;

		if (CanInherit())
			Value = ParentAsProperty.Value;
			
		ParentAsProperty.PropertyValueChanged += OnParentPropertyValueChange;
	}


	public bool CanInherit() => Inherit && ParentAsProperty is not null && ParentAsProperty.CanBeInherited;

	public void InheritValue()
		=> Value = ParentAsProperty is not null ? ParentAsProperty.Value : Value;

	public void InheritValueIfPossible()
	{
		if (CanInherit())
			InheritValue();
	}


	public static implicit operator T(InheritableComponentProperty<T> property) => property.Value;
	public static implicit operator InheritableComponentProperty<T>(T value) => new(value);


    public virtual void OnParentPropertyValueChange(T newValue) => InheritValueIfPossible();


    public virtual void Update() => InheritValueIfPossible();
}