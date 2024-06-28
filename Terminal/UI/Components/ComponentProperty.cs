namespace Specter.Terminal.UI.Components;


/// <summary>
/// Represents properties of UI components, but with extended behavior.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
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



/// <summary>
/// Provides some basic inheritance behavior.
/// </summary>
public interface IInheritable : IUpdateable
{
	public IInheritable? Parent { get; set; }

	public bool Inherit { get; set; }
	public bool ConstantValueUpdate { get; set; }
	public bool CanBeInherited { get; set; }


	public void InheritValue();
}


/// <summary>
/// ComponentProperty that can inherit values from its parent.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class InheritableComponentProperty<T> : ComponentProperty<T>, IInheritable
{
    public IInheritable? Parent { get; set; }

	/// <summary>
	/// Tries to convert Parent to a component property.
	/// </summary>
	public InheritableComponentProperty<T>? ParentAsProperty
	{
		get => Parent as InheritableComponentProperty<T>;
	}


	// * Value should not be modified by this object.
	// * Use 'DefaultValue' instead.
    new public T Value
	{
		get => base.Value;
		private set => base.Value = value;
	}

	/// <summary>
	/// Default value to be set to 'Value'.
	/// Ignored if inheriting value from 'Parent'.
	/// </summary>
    public T DefaultValue { get; set; }


	public bool Inherit { get; set; }
	public bool CanBeInherited { get; set; }
	public bool ConstantValueUpdate { get; set; }


	// TODO: set inheriting disabled by default
	public InheritableComponentProperty(

		T defaultValue,
		InheritableComponentProperty<T>? parent = null,
		bool inherit = true,
		bool canBeInherited = true

		
	) : base(defaultValue)
	{
		Parent = parent;
	
		DefaultValue = defaultValue;

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

	protected bool InheritValueIfPossible()
	{
		if (CanInherit())
		{
			InheritValue();
			return true;
		}

		return false;
	}


	public static implicit operator T(InheritableComponentProperty<T> property) => property.DefaultValue;
	public static implicit operator InheritableComponentProperty<T>(T value) => new(value);


    public virtual void OnParentPropertyValueChange(T newValue) => InheritValueIfPossible();


    public virtual void Update()
	{
		// use DefaultValue if inheritance is not possible
		if (!InheritValueIfPossible())
			Value = DefaultValue;
	}
}