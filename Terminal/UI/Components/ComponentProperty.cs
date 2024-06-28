namespace Specter.Terminal.UI.Components;


/// <summary>
/// Represents properties of UI components, but with extended behavior.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class ComponentProperty<T> : IUpdateable
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

	public T DefaultValue { get; set; }


	public ComponentProperty<T>? LinkProperty { get; set; }
	public bool UseLink { get; set; }


	public ComponentProperty(ComponentProperty<T> link) : this(link.Value)
	{
		LinkProperty = link;
		UseLink = true;
	}

	public ComponentProperty(T value)
	{
		_value = DefaultValue = value;
		UseLink = false;

		PropertyValueChanged += OnPropertyValueChange;
	}


	public bool CanLink() => UseLink && LinkProperty is not null;

	public void LinkValue() => Value = LinkProperty is not null ? LinkProperty.Value : Value;


	public static implicit operator T(ComponentProperty<T> property) => property.Value;
	public static implicit operator ComponentProperty<T>(T value) => new(value);
	

	public delegate void ValueChangedEventHandler(T newValue);

	public event ValueChangedEventHandler? PropertyValueChanged;

	public void RaiseValueChangedEvent(T newValue) => PropertyValueChanged?.Invoke(newValue);


	public virtual void OnPropertyValueChange(T newValue) {}


	public virtual void Update()
	{
		if (CanLink())
			LinkValue();
		else
			Value = DefaultValue;
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
		bool canBeInherited = true

		
	) : base(value)
	{
		Parent = parent;

		Inherit = inherit;
		CanBeInherited = canBeInherited;

		if (ParentAsProperty is null)
			return;

		if (CanInherit())
			Value = ParentAsProperty.Value;
			
		ParentAsProperty.PropertyValueChanged += OnParentPropertyValueChange;
	}

	public InheritableComponentProperty(

		ComponentProperty<T> link,
		InheritableComponentProperty<T>? parent = null,
		bool inherit = true,
		bool canBeInherited = true

		
	) : this(link.Value, parent, inherit, canBeInherited)
	{
		LinkProperty = link;
		UseLink = true;
	}

	// * Inheriting have a higher priority than Linking. If CanLink and CanInherit,
	// * Inherit will be considered


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


    public override void Update()
	{
		base.Update();

		InheritValueIfPossible();
	}
}