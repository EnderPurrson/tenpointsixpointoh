using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Prime31.Reflection
{
	public class ReflectionUtils
	{
		public static global::System.Attribute getAttribute(MemberInfo info, global::System.Type type)
		{
			if (info == null || type == null || !global::System.Attribute.IsDefined(info, type))
			{
				return null;
			}
			return global::System.Attribute.GetCustomAttribute(info, type);
		}

		public static global::System.Attribute getAttribute(global::System.Type objectType, global::System.Type attributeType)
		{
			if (objectType == null || attributeType == null || !global::System.Attribute.IsDefined((MemberInfo)(object)objectType, attributeType))
			{
				return null;
			}
			return global::System.Attribute.GetCustomAttribute((MemberInfo)(object)objectType, attributeType);
		}

		public static bool isTypeGenericeCollectionInterface(global::System.Type type)
		{
			if (!type.get_IsGenericType())
			{
				return false;
			}
			global::System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(global::System.Collections.Generic.IList<>) || genericTypeDefinition == typeof(global::System.Collections.Generic.ICollection<>) || genericTypeDefinition == typeof(global::System.Collections.Generic.IEnumerable<>);
		}

		public static bool isTypeDictionary(global::System.Type type)
		{
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (!type.get_IsGenericType())
			{
				return false;
			}
			global::System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary<, >);
		}

		public static bool isNullableType(global::System.Type type)
		{
			return type.get_IsGenericType() && type.GetGenericTypeDefinition() == typeof(global::System.Nullable<>);
		}

		public static object toNullableType(object obj, global::System.Type nullableType)
		{
			return (obj != null) ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), (IFormatProvider)(object)CultureInfo.get_InvariantCulture()) : null;
		}
	}
}
