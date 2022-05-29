using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

[Serializable]
public class PropertyReference
{
	[SerializeField]
	private Component mTarget;

	[SerializeField]
	private string mName;

	private FieldInfo mField;

	private PropertyInfo mProperty;

	private static int s_Hash;

	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return (monoBehaviour == null ? true : monoBehaviour.enabled);
		}
	}

	public bool isValid
	{
		get
		{
			return (this.mTarget == null ? false : !string.IsNullOrEmpty(this.mName));
		}
	}

	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	static PropertyReference()
	{
		PropertyReference.s_Hash = "PropertyBinding".GetHashCode();
	}

	public PropertyReference()
	{
	}

	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private bool Cache()
	{
		if (!(this.mTarget != null) || string.IsNullOrEmpty(this.mName))
		{
			this.mField = null;
			this.mProperty = null;
		}
		else
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetField(this.mName);
			this.mProperty = type.GetProperty(this.mName);
		}
		return (this.mField != null ? true : this.mProperty != null);
	}

	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	private bool Convert(ref object value)
	{
		Type type;
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		if (value != null)
		{
			type = value.GetType();
		}
		else
		{
			if (!propertyType.IsClass)
			{
				return false;
			}
			type = propertyType;
		}
		return PropertyReference.Convert(ref value, type, propertyType);
	}

	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	public static bool Convert(object value, Type to)
	{
		if (value != null)
		{
			return PropertyReference.Convert(ref value, value.GetType(), to);
		}
		value = null;
		return PropertyReference.Convert(ref value, to, to);
	}

	public static bool Convert(ref object value, Type from, Type to)
	{
		int num;
		float single;
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = (value == null ? "null" : value.ToString());
			return true;
		}
		if (value == null)
		{
			return false;
		}
		if (to != typeof(int))
		{
			if (to == typeof(float) && from == typeof(string) && float.TryParse((string)value, out single))
			{
				value = single;
				return true;
			}
		}
		else if (from == typeof(string))
		{
			if (int.TryParse((string)value, out num))
			{
				value = num;
				return true;
			}
		}
		else if (from == typeof(float))
		{
			value = Mathf.RoundToInt((float)value);
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (!(obj is PropertyReference))
		{
			return false;
		}
		PropertyReference propertyReference = obj as PropertyReference;
		return (this.mTarget != propertyReference.mTarget ? false : string.Equals(this.mName, propertyReference.mName));
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField == null)
		{
			return typeof(void);
		}
		return this.mField.FieldType;
	}

	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public bool Set(object value)
	{
		bool flag;
		object[] type;
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (this.mProperty == null)
				{
					this.mField.SetValue(this.mTarget, null);
					flag = true;
				}
				else if (!this.mProperty.CanWrite)
				{
					if (this.Convert(ref value))
					{
						if (this.mField != null)
						{
							this.mField.SetValue(this.mTarget, value);
							return true;
						}
						if (this.mProperty.CanWrite)
						{
							this.mProperty.SetValue(this.mTarget, value, null);
							return true;
						}
					}
					else if (Application.isPlaying)
					{
						type = new object[] { "Unable to convert ", value.GetType(), " to ", this.GetPropertyType() };
						UnityEngine.Debug.LogError(string.Concat(type));
					}
					return false;
				}
				else
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					flag = true;
				}
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}
		if (this.Convert(ref value))
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		else if (Application.isPlaying)
		{
			type = new object[] { "Unable to convert ", value.GetType(), " to ", this.GetPropertyType() };
			UnityEngine.Debug.LogError(string.Concat(type));
		}
		return false;
	}

	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	public static string ToString(Component comp, string property)
	{
		if (comp == null)
		{
			return null;
		}
		string str = comp.GetType().ToString();
		int num = str.LastIndexOf('.');
		if (num > 0)
		{
			str = str.Substring(num + 1);
		}
		if (string.IsNullOrEmpty(property))
		{
			return string.Concat(str, ".[property]");
		}
		return string.Concat(str, ".", property);
	}
}