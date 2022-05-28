using System;
using System.Reflection;

namespace Prime31.Reflection
{
	public class CacheResolver
	{
		private delegate object CtorDelegate();

		public sealed class MemberMap
		{
			public readonly MemberInfo MemberInfo;

			public readonly global::System.Type Type;

			public readonly GetHandler Getter;

			public readonly SetHandler Setter;

			public MemberMap(PropertyInfo propertyInfo)
			{
				MemberInfo = (MemberInfo)(object)propertyInfo;
				Type = propertyInfo.get_PropertyType();
				Getter = createGetHandler(propertyInfo);
				Setter = createSetHandler(propertyInfo);
			}

			public MemberMap(FieldInfo fieldInfo)
			{
				MemberInfo = (MemberInfo)(object)fieldInfo;
				Type = fieldInfo.get_FieldType();
				Getter = createGetHandler(fieldInfo);
				Setter = createSetHandler(fieldInfo);
			}
		}

		private readonly MemberMapLoader _memberMapLoader;

		private readonly SafeDictionary<global::System.Type, SafeDictionary<string, MemberMap>> _memberMapsCache = new SafeDictionary<global::System.Type, SafeDictionary<string, MemberMap>>();

		private static readonly SafeDictionary<global::System.Type, CtorDelegate> constructorCache = new SafeDictionary<global::System.Type, CtorDelegate>();

		public CacheResolver(MemberMapLoader memberMapLoader)
		{
			_memberMapLoader = memberMapLoader;
		}

		public static object getNewInstance(global::System.Type type)
		{
			_003CgetNewInstance_003Ec__AnonStorey0 CS_0024_003C_003E8__locals0 = new _003CgetNewInstance_003Ec__AnonStorey0();
			CtorDelegate value;
			if (constructorCache.tryGetValue(type, out value))
			{
				return value();
			}
			CS_0024_003C_003E8__locals0.constructorInfo = type.GetConstructor((BindingFlags)52, (Binder)null, global::System.Type.EmptyTypes, (ParameterModifier[])null);
			value = () => CS_0024_003C_003E8__locals0.constructorInfo.Invoke((object[])null);
			constructorCache.add(type, value);
			return value();
		}

		public SafeDictionary<string, MemberMap> loadMaps(global::System.Type type)
		{
			if (type == null || type == typeof(object))
			{
				return null;
			}
			SafeDictionary<string, MemberMap> value;
			if (_memberMapsCache.tryGetValue(type, out value))
			{
				return value;
			}
			value = new SafeDictionary<string, MemberMap>();
			_memberMapLoader(type, value);
			_memberMapsCache.add(type, value);
			return value;
		}

		private static GetHandler createGetHandler(FieldInfo fieldInfo)
		{
			_003CcreateGetHandler_003Ec__AnonStorey1 CS_0024_003C_003E8__locals0 = new _003CcreateGetHandler_003Ec__AnonStorey1();
			CS_0024_003C_003E8__locals0.fieldInfo = fieldInfo;
			return (object instance) => CS_0024_003C_003E8__locals0.fieldInfo.GetValue(instance);
		}

		private static SetHandler createSetHandler(FieldInfo fieldInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey2 CS_0024_003C_003E8__locals0 = new _003CcreateSetHandler_003Ec__AnonStorey2();
			CS_0024_003C_003E8__locals0.fieldInfo = fieldInfo;
			if (CS_0024_003C_003E8__locals0.fieldInfo.get_IsInitOnly() || CS_0024_003C_003E8__locals0.fieldInfo.get_IsLiteral())
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				CS_0024_003C_003E8__locals0.fieldInfo.SetValue(instance, value);
			};
		}

		private static GetHandler createGetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateGetHandler_003Ec__AnonStorey3 CS_0024_003C_003E8__locals0 = new _003CcreateGetHandler_003Ec__AnonStorey3();
			CS_0024_003C_003E8__locals0.getMethodInfo = propertyInfo.GetGetMethod(true);
			if (CS_0024_003C_003E8__locals0.getMethodInfo == null)
			{
				return null;
			}
			return (object instance) => ((MethodBase)CS_0024_003C_003E8__locals0.getMethodInfo).Invoke(instance, (object[])global::System.Type.EmptyTypes);
		}

		private static SetHandler createSetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey4 CS_0024_003C_003E8__locals0 = new _003CcreateSetHandler_003Ec__AnonStorey4();
			CS_0024_003C_003E8__locals0.setMethodInfo = propertyInfo.GetSetMethod(true);
			if (CS_0024_003C_003E8__locals0.setMethodInfo == null)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				((MethodBase)CS_0024_003C_003E8__locals0.setMethodInfo).Invoke(instance, new object[1] { value });
			};
		}
	}
}
