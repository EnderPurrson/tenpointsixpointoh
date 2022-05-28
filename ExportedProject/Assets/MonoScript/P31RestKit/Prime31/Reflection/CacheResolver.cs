using System;
using System.Reflection;
using System.Runtime.CompilerServices;

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

		[CompilerGenerated]
		private sealed class _003CgetNewInstance_003Ec__AnonStorey0
		{
			internal ConstructorInfo constructorInfo;

			internal object _003C_003Em__0()
			{
				return constructorInfo.Invoke((object[])default(object[]));
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateGetHandler_003Ec__AnonStorey1
		{
			internal FieldInfo fieldInfo;

			internal object _003C_003Em__0(object instance)
			{
				return fieldInfo.GetValue(instance);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateSetHandler_003Ec__AnonStorey2
		{
			internal FieldInfo fieldInfo;

			internal void _003C_003Em__0(object instance, object value)
			{
				fieldInfo.SetValue(instance, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateGetHandler_003Ec__AnonStorey3
		{
			internal MethodInfo getMethodInfo;

			internal object _003C_003Em__0(object instance)
			{
				return ((MethodBase)getMethodInfo).Invoke(instance, (object[])global::System.Type.EmptyTypes);
			}
		}

		[CompilerGenerated]
		private sealed class _003CcreateSetHandler_003Ec__AnonStorey4
		{
			internal MethodInfo setMethodInfo;

			internal void _003C_003Em__0(object instance, object value)
			{
				((MethodBase)setMethodInfo).Invoke(instance, new object[1] { value });
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
			_003CgetNewInstance_003Ec__AnonStorey0 _003CgetNewInstance_003Ec__AnonStorey = new _003CgetNewInstance_003Ec__AnonStorey0();
			CtorDelegate value;
			if (constructorCache.tryGetValue(type, out value))
			{
				return value();
			}
			_003CgetNewInstance_003Ec__AnonStorey.constructorInfo = type.GetConstructor((BindingFlags)52, (Binder)default(Binder), global::System.Type.EmptyTypes, (ParameterModifier[])default(ParameterModifier[]));
			value = _003CgetNewInstance_003Ec__AnonStorey._003C_003Em__0;
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
			_003CcreateGetHandler_003Ec__AnonStorey1 _003CcreateGetHandler_003Ec__AnonStorey = new _003CcreateGetHandler_003Ec__AnonStorey1();
			_003CcreateGetHandler_003Ec__AnonStorey.fieldInfo = fieldInfo;
			return _003CcreateGetHandler_003Ec__AnonStorey._003C_003Em__0;
		}

		private static SetHandler createSetHandler(FieldInfo fieldInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey2 _003CcreateSetHandler_003Ec__AnonStorey = new _003CcreateSetHandler_003Ec__AnonStorey2();
			_003CcreateSetHandler_003Ec__AnonStorey.fieldInfo = fieldInfo;
			if (_003CcreateSetHandler_003Ec__AnonStorey.fieldInfo.get_IsInitOnly() || _003CcreateSetHandler_003Ec__AnonStorey.fieldInfo.get_IsLiteral())
			{
				return null;
			}
			return _003CcreateSetHandler_003Ec__AnonStorey._003C_003Em__0;
		}

		private static GetHandler createGetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateGetHandler_003Ec__AnonStorey3 _003CcreateGetHandler_003Ec__AnonStorey = new _003CcreateGetHandler_003Ec__AnonStorey3();
			_003CcreateGetHandler_003Ec__AnonStorey.getMethodInfo = propertyInfo.GetGetMethod(true);
			if (_003CcreateGetHandler_003Ec__AnonStorey.getMethodInfo == null)
			{
				return null;
			}
			return _003CcreateGetHandler_003Ec__AnonStorey._003C_003Em__0;
		}

		private static SetHandler createSetHandler(PropertyInfo propertyInfo)
		{
			_003CcreateSetHandler_003Ec__AnonStorey4 _003CcreateSetHandler_003Ec__AnonStorey = new _003CcreateSetHandler_003Ec__AnonStorey4();
			_003CcreateSetHandler_003Ec__AnonStorey.setMethodInfo = propertyInfo.GetSetMethod(true);
			if (_003CcreateSetHandler_003Ec__AnonStorey.setMethodInfo == null)
			{
				return null;
			}
			return _003CcreateSetHandler_003Ec__AnonStorey._003C_003Em__0;
		}
	}
}
