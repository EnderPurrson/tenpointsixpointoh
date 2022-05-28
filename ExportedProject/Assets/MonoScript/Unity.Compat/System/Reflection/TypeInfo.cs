using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	public class TypeInfo : System.Type
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1_0
		{
			public System.Type type;

			internal TypeInfo _003CFromType_003Eb__0(System.Type _)
			{
				return new TypeInfo(type);
			}
		}

		private static ConditionalWeakTable<System.Type, TypeInfo> typeInfoMap = new ConditionalWeakTable<System.Type, TypeInfo>();

		private System.Type underlyingType;

		public System.Collections.Generic.IEnumerable<System.Type> ImplementedInterfaces
		{
			get
			{
				return underlyingType.GetInterfaces();
			}
		}

		public override Assembly Assembly
		{
			get
			{
				return underlyingType.get_Assembly();
			}
		}

		public override string AssemblyQualifiedName
		{
			get
			{
				return underlyingType.get_AssemblyQualifiedName();
			}
		}

		public override System.Type BaseType
		{
			get
			{
				return underlyingType.get_BaseType();
			}
		}

		public override string FullName
		{
			get
			{
				return underlyingType.get_FullName();
			}
		}

		public override Guid GUID
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return underlyingType.get_GUID();
			}
		}

		public override Module Module
		{
			get
			{
				return underlyingType.get_Module();
			}
		}

		public override string Namespace
		{
			get
			{
				return underlyingType.get_Namespace();
			}
		}

		public override System.Type UnderlyingSystemType
		{
			get
			{
				return underlyingType.get_UnderlyingSystemType();
			}
		}

		public override string Name
		{
			get
			{
				return ((MemberInfo)underlyingType).get_Name();
			}
		}

		internal static TypeInfo FromType(System.Type type)
		{
			_003C_003Ec__DisplayClass1_0 _003C_003Ec__DisplayClass1_ = new _003C_003Ec__DisplayClass1_0();
			_003C_003Ec__DisplayClass1_.type = type;
			return typeInfoMap.GetValue(_003C_003Ec__DisplayClass1_.type, _003C_003Ec__DisplayClass1_._003CFromType_003Eb__0);
		}

		private TypeInfo(System.Type underlyingType)
		{
			this.underlyingType = underlyingType;
		}

		public T GetCustomAttribute<T>(bool inherit) where T : System.Attribute
		{
			return (T)Enumerable.FirstOrDefault<object>((System.Collections.Generic.IEnumerable<object>)((MemberInfo)underlyingType).GetCustomAttributes(typeof(T), inherit));
		}

		public T GetCustomAttribute<T>() where T : System.Attribute
		{
			return GetCustomAttribute<T>(true);
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.get_Attributes();
		}

		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, System.Type[] types, ParameterModifier[] modifiers)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetConstructors(bindingAttr);
		}

		public override System.Type GetElementType()
		{
			return underlyingType.GetElementType();
		}

		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetEvent(name, bindingAttr);
		}

		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetEvents(bindingAttr);
		}

		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetField(name, bindingAttr);
		}

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetFields(bindingAttr);
		}

		public override System.Type GetInterface(string name, bool ignoreCase)
		{
			return underlyingType.GetInterface(name, ignoreCase);
		}

		public override System.Type[] GetInterfaces()
		{
			return underlyingType.GetInterfaces();
		}

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetMembers(bindingAttr);
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, System.Type[] types, ParameterModifier[] modifiers)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetMethods(bindingAttr);
		}

		public override System.Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetNestedType(name, bindingAttr);
		}

		public override System.Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetNestedTypes(bindingAttr);
		}

		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetProperties(bindingAttr);
		}

		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, System.Type returnType, System.Type[] types, ParameterModifier[] modifiers)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		protected override bool HasElementTypeImpl()
		{
			return underlyingType.get_HasElementType();
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return underlyingType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		protected override bool IsArrayImpl()
		{
			return underlyingType.get_IsArray();
		}

		protected override bool IsByRefImpl()
		{
			return underlyingType.get_IsByRef();
		}

		protected override bool IsCOMObjectImpl()
		{
			return underlyingType.get_IsCOMObject();
		}

		protected override bool IsPointerImpl()
		{
			return underlyingType.get_IsPointer();
		}

		protected override bool IsPrimitiveImpl()
		{
			return underlyingType.get_IsPrimitive();
		}

		public override object[] GetCustomAttributes(System.Type attributeType, bool inherit)
		{
			return ((MemberInfo)underlyingType).GetCustomAttributes(attributeType, inherit);
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			return ((MemberInfo)underlyingType).GetCustomAttributes(inherit);
		}

		public override bool IsDefined(System.Type attributeType, bool inherit)
		{
			return ((MemberInfo)underlyingType).IsDefined(attributeType, inherit);
		}
	}
}
