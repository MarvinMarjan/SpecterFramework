namespace Specter.Terminal.UI.Components;


public interface IComponentPropertyEvents<T>
{
	delegate void ValueChangedEventHandler(T newValue);

	event ValueChangedEventHandler? PropertyValueChanged;
}


public interface IComponentPropertyEvents
{
	delegate void ValueChangedEventHandler(object newValue);

	event ValueChangedEventHandler? PropertyValueChanged;
}


/// <summary>
/// Represents properties of UI components, but with extended behavior.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class ComponentProperty<T> : IComponentPropertyEvents<T>, IComponentPropertyEvents, IUpdateable
	where T : notnull
{
    private T _value;
    public T Value
	{
		get => _value;
		protected set
		{
			_value = value;
			RaiseValueChangedEvent(value);
		}
	}

	private T _defaultValue;
	public T DefaultValue
	{
		get => _defaultValue;
		set
		{
			if (UpdateOnChange && !value.Equals(Value))
				Value = value;

			_defaultValue = value;
		}
	}

	public bool UpdateOnChange { get; set; }



	public ComponentProperty<T>? LinkProperty { get; set; }
	public bool UseLink { get; set; }


	public ComponentProperty(ComponentProperty<T> link, bool updateOnChange)
		: this(link.Value, updateOnChange)
	{
		LinkProperty = link;
		UseLink = true;
	}

	public ComponentProperty(T value, bool updateOnChange = false)
	{
		_value = _defaultValue = value;
		UpdateOnChange = updateOnChange;
		UseLink = false;

		PropertyValueChanged += OnPropertyValueChange;
		PropertyObjectValueChanged += OnPropertyObjectValueChange;
	}



    public bool CanLink() => UseLink && LinkProperty is not null;

	public void LinkValue()
	{
		if (LinkProperty is not null && !LinkProperty.Value.Equals(Value))
			Value = LinkProperty.Value;
	}


	public void SetValueToDefault()
	{
		if (!DefaultValue.Equals(Value))
			Value = DefaultValue;
	}


	public static implicit operator T(ComponentProperty<T> property) => property.Value;
	public static implicit operator ComponentProperty<T>(T value) => new(value);
	

	// Events

	public event IComponentPropertyEvents<T>.ValueChangedEventHandler? PropertyValueChanged;
	event IComponentPropertyEvents<T>.ValueChangedEventHandler? IComponentPropertyEvents<T>.PropertyValueChanged
	{
		add => PropertyValueChanged += value;
		remove => PropertyValueChanged -= value;
	}


	public event IComponentPropertyEvents.ValueChangedEventHandler? PropertyObjectValueChanged;
	event IComponentPropertyEvents.ValueChangedEventHandler? IComponentPropertyEvents.PropertyValueChanged
	{
		add => PropertyObjectValueChanged += value;
		remove => PropertyObjectValueChanged -= value;
	}


	protected void RaiseValueChangedEvent(T newValue)
	{
		PropertyValueChanged?.Invoke(newValue);
		PropertyObjectValueChanged?.Invoke(newValue);
	}


	protected virtual void OnPropertyValueChange(T newValue) {}
	protected virtual void OnPropertyObjectValueChange(object newValue) {}




	public virtual void Update()
	{
		if (CanLink())
			LinkValue();
		else
			SetValueToDefault();
			
		// ! Default values are setted to Value only in updates.
		// ! If you need to do this immediately, use 'UpdateOnChange = true'.
	}
}



/// <summary>
/// Provides some basic inheritance behavior.
/// </summary>
public interface IInheritable
{
	public IInheritable? Parent { get; set; }

	public bool Inherit { get; set; }
	public bool CanBeInherited { get; set; }


	public void InheritValue();
}


/// <summary>
/// ComponentProperty that can inherit values from its parent.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class InheritableComponentProperty<T> : ComponentProperty<T>, IInheritable
	where T : notnull
{
    public IInheritable? Parent { get; set; }

	/// <summary>
	/// Tries to convert Parent to a component property.
	/// </summary>
	public InheritableComponentProperty<T>? ParentAsProperty => Parent as InheritableComponentProperty<T>;


	public bool Inherit { get; set; }
	public bool CanBeInherited { get; set; }


	public InheritableComponentProperty(

		T value,
		InheritableComponentProperty<T>? parent = null,
		bool inherit = true,
		bool canBeInherited = true,
		bool updateOnChange = false

		
	) : base(value, updateOnChange)
	{
		Parent = parent;

		Inherit = inherit;
		CanBeInherited = canBeInherited;

		if (ParentAsProperty is null)
			return;

		if (CanInherit())
			Value = ParentAsProperty.Value;
			
		ParentAsProperty.PropertyValueChanged += OnParentPropertyValueChange;
		ParentAsProperty.PropertyObjectValueChanged += OnParentPropertyObjectValueChange;
	}

	public InheritableComponentProperty(

		ComponentProperty<T> link,
		InheritableComponentProperty<T>? parent = null,
		bool inherit = true,
		bool canBeInherited = true,
		bool updateOnChange = false

		
	) : this(link.Value, parent, inherit, canBeInherited, updateOnChange)
	{
		LinkProperty = link;
		UseLink = true;
	}

	// * Inheriting have a higher priority than Linking. If CanLink and CanInherit,
	// * Inherit will be considered


	public bool CanInherit() => Inherit && ParentAsProperty is not null && ParentAsProperty.CanBeInherited;

	public void InheritValue()
	{
		if (ParentAsProperty is not null && !ParentAsProperty.Value.Equals(Value))
			Value = ParentAsProperty.Value;
	}

	public void InheritValueIfPossible()
	{
		if (CanInherit())
			InheritValue();
	}


	public static implicit operator T(InheritableComponentProperty<T> property) => property.Value;
	public static implicit operator InheritableComponentProperty<T>(T value) => new(value);


    public virtual void OnParentPropertyValueChange(T newValue) => InheritValueIfPossible();
    public virtual void OnParentPropertyObjectValueChange(object newValue) {}

    public override void Update()
	{
		// linking of default value
		base.Update();

		// inheriting
		InheritValueIfPossible();
	}
}