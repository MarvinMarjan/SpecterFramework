using Specter.Terminal.UI.Application;
using Specter.Terminal.UI.Application.Exceptions;


namespace Specter.Terminal.UI.Components.Property;


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



public readonly struct ComponentPropertyData(string name, string? typeName, Component? owner)
{
	public string Name { get; init; } = name;
	public string? TypeName { get; init; } = typeName;
	public Component? Owner { get; init; } = owner;
}



/// <summary>
/// The base class of every other ComponentProperty.
/// </summary>
public abstract class ComponentProperty
{
	/// <summary>
	/// The Component that owns and manage this property.
	/// </summary>
	public Component Owner { get; set; }
	public ComponentPropertiesManager Manager => Owner.PropertiesManager;
	public bool HasManager => Manager is not null;

	/// <summary>
	/// Name of the property.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// The value of the property as raw Object.
	/// </summary>
	public abstract object ValueObject { get; }


	private ComponentPropertyAttributes _attributes;
	public ComponentPropertyAttributes Attributes
	{
		get => _attributes;
		set
		{
			if (this is not IInheritable)
			{
				_attributes = value;
				return;
			}

			if (value is not InheritableComponentPropertyAttributes inheritableAttribute)
				throw new ComponentPropertyException(
					GetData(),
					@"An ""InheritableComponentProperty<T>"" attribute must be an ""InheritableComponentPropertyAttribute""."
				);

			_attributes = inheritableAttribute;
		}
	}


	public ComponentProperty(
		
		Component owner,
		string name,
		ComponentPropertyAttributes attributes
	
	)
	{
		_attributes = attributes; // * initialize Attributes before 'Manager.Add()'.

		Owner = owner;
		Manager.Add(this);

		Name = name;
	}



	public ComponentPropertyData GetData()
		=> new(Name, GetType().Name, Owner);
}


/// <summary>
/// A generic ComponentProperty with extended behaviour. 
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class ComponentProperty<T>
	: ComponentProperty, IComponentPropertyEvents<T>, IComponentPropertyEvents, IUpdateable
		where T : notnull
{
	public override object ValueObject => Value;


	private T _value;

	/// <summary>
	/// The typed value of this property.
	/// </summary>
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

	/// <summary>
	/// The default value to be set at every update call.
	/// Between inheriting a value and setting a default value,
	/// inheriting have a higher priority, so the default value is ignored.
	/// </summary>
	public T DefaultValue
	{
		get => _defaultValue;
		set
		{
			if (Attributes.UpdateOnChange && !value.Equals(Value))
				Value = value;

			_defaultValue = value;
		}
	}



	/// <summary>
	/// Defines another ComponentProperty this property should
	/// copy the value. Has a simillar behaviour to DefaultValue.
	/// </summary>
	public ComponentProperty<T>? LinkProperty { get; set; }
	public bool UseLink { get; set; }


	public ComponentProperty(
	
		Component owner,
		string name,
		T value,
		ComponentPropertyAttributes attributes

	) : base(owner, name, attributes)
	{
		_value = _defaultValue = value;

		UseLink = false;

		PropertyValueChanged += OnPropertyValueChange;
		PropertyObjectValueChanged += OnPropertyObjectValueChange;
	}


	public ComponentProperty(
		
		Component owner,
		string name,
		ComponentProperty<T> link,
		ComponentPropertyAttributes attributes

	) : this(owner, name, link.Value, attributes	)
	{
		LinkProperty = link;
		UseLink = true;
	}



	public bool CanLink() => UseLink && LinkProperty is not null;

	public void LinkValue()
	{
		if (LinkProperty is not null && !LinkProperty.Value.Equals(Value))
			Value = LinkProperty.Value;
	}


	/// <summary>
	/// Sets the value of DefaultValue to Value.
	/// </summary>
	public void SetValueToDefault()
	{
		if (!DefaultValue.Equals(Value))
			Value = DefaultValue;
	}


	public static implicit operator T(ComponentProperty<T> property) => property.Value;
	

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


	protected virtual void OnPropertyValueChange(T newValue)
	{
		if (!Attributes.RequestOwnerRenderOnPropertyChange)
			return;

		if (Attributes.DrawAllRequest)
			App.RequestDrawAll();
		else
			Owner.AddThisToRenderQueue();
	}

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
/// Provides behaviour for inheriting a value from a parent.
/// </summary>
public interface IInheritable
{
	public IInheritable? Parent { get; set; }

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

	public InheritableComponentProperty<T>? ParentAsProperty => Parent as InheritableComponentProperty<T>;


	new public InheritableComponentPropertyAttributes Attributes
	{
		get
		{
			// if the base.Attributes runtime type is ComponentPropertyAttributes, then
			// its values are copied to a default InheritableComponentPropertyAttributes.
			// So the return is a InheritableComponentPropertyAttributes with base.Attributes
			// values.

			if (base.Attributes is not InheritableComponentPropertyAttributes inheritableTypeAttributes)
				return (InheritableComponentPropertyAttributes)base.Attributes;

			return inheritableTypeAttributes;
		}

		set
		{
			if (value is not null)
				base.Attributes = value;
		}
	}


	public InheritableComponentProperty(

		Component owner,
		string name,
		T value,
		InheritableComponentProperty<T>? parent,
		InheritableComponentPropertyAttributes attributes
		
	) : base(
		owner, name, value, attributes
	)
	{
		Parent = parent;

		if (ParentAsProperty is null)
			return;

		if (CanInherit())
			Value = ParentAsProperty.Value;
			
		ParentAsProperty.PropertyValueChanged += OnParentPropertyValueChange;
		ParentAsProperty.PropertyObjectValueChanged += OnParentPropertyObjectValueChange;
	}

	public InheritableComponentProperty(

		Component owner,
		string name,
		ComponentProperty<T> link,
		InheritableComponentProperty<T>? parent,
		InheritableComponentPropertyAttributes attributes
		
	) : this(owner, name, link.Value, parent, attributes)
	{
		LinkProperty = link;
		UseLink = true;
	}

	// * Inheriting have a higher priority than Linking. If CanLink and CanInherit,
	// * Inherit will be considered


	public bool CanInherit()
		=> Attributes.Inherit && ParentAsProperty is not null && ParentAsProperty.Attributes.CanBeInherited;


	/// <summary>
	/// Forces the value inheriting
	/// </summary>
	public void InheritValue()
	{
		if (ParentAsProperty is not null && !ParentAsProperty.Value.Equals(Value))
			Value = ParentAsProperty.Value;
	}


	public bool TryInheritValue()
	{
		if (CanInherit())
		{
			InheritValue();
			return true;
		}

		return false;
	}


	public static implicit operator T(InheritableComponentProperty<T> property) => property.Value;


	public virtual void OnParentPropertyValueChange(T newValue) => TryInheritValue();
	public virtual void OnParentPropertyObjectValueChange(object newValue) {}

	public override void Update()
	{
		// linking of default value
		base.Update();

		// inheriting
		TryInheritValue();
	}
}
