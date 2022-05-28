using System;
using System.Collections;
using Boo.Lang;
using Boo.Lang.Runtime;

namespace UnityScript.Lang
{
	[Serializable]
	public class UnityRuntimeServices
	{
		[Serializable]
		public abstract class ValueTypeChange
		{
			public object Target;

			public object Value;

			public bool IsValid
			{
				get
				{
					return Value is ValueType;
				}
			}

			public ValueTypeChange(object target, object value)
			{
				Target = target;
				Value = value;
			}

			public abstract override bool Propagate();
		}

		[Serializable]
		public class MemberValueTypeChange : ValueTypeChange
		{
			public string Member;

			public MemberValueTypeChange(object target, string member, object value)
				: base(target, value)
			{
				Member = member;
			}

			public override bool Propagate()
			{
				//Discarded unreachable code: IL_0036
				int result;
				bool flag;
				if (!IsValid)
				{
					result = 0;
				}
				else
				{
					try
					{
						RuntimeServices.SetProperty(Target, Member, Value);
					}
					catch (MissingFieldException)
					{
						flag = false;
						goto IL_003e;
					}
					result = 1;
				}
				goto IL_003f;
				IL_003e:
				result = (flag ? 1 : 0);
				goto IL_003f;
				IL_003f:
				return (byte)result != 0;
			}
		}

		[Serializable]
		public class SliceValueTypeChange : ValueTypeChange
		{
			public object Index;

			public SliceValueTypeChange(object target, object index, object value)
				: base(target, value)
			{
				Index = index;
			}

			public override bool Propagate()
			{
				//Discarded unreachable code: IL_0076
				int result;
				bool flag;
				if (!IsValid)
				{
					result = 0;
				}
				else
				{
					IList list = Target as IList;
					if (list != null)
					{
						list[RuntimeServices.UnboxInt32(Index)] = Value;
						result = 1;
					}
					else
					{
						try
						{
							RuntimeServices.SetSlice(Target, string.Empty, new object[2] { Index, Value });
						}
						catch (MissingFieldException)
						{
							flag = false;
							goto IL_007e;
						}
						result = 1;
					}
				}
				goto IL_007f;
				IL_007e:
				result = (flag ? 1 : 0);
				goto IL_007f;
				IL_007f:
				return (byte)result != 0;
			}
		}

		[NonSerialized]
		public static IEnumerator EmptyEnumerator;

		[NonSerialized]
		protected static Type EnumeratorType;

		[NonSerialized]
		public static readonly bool Initialized;

		static UnityRuntimeServices()
		{
			_0024static_initializer_0024();
			RuntimeServices.RegisterExtensions(typeof(Extensions));
			Initialized = true;
		}

		public static object Invoke(object target, string name, object[] args, Type scriptBaseType)
		{
			if (!Initialized)
			{
				throw new AssertionFailedException("Initialized");
			}
			object obj = RuntimeServices.Invoke(target, name, args);
			return (obj == null) ? null : ((!IsGenerator(obj)) ? obj : ((!target.GetType().IsSubclassOf(scriptBaseType)) ? obj : ((!IsStaticMethod(target.GetType(), name, args)) ? RuntimeServices.Invoke(target, "StartCoroutine_Auto", new object[1] { obj }) : obj)));
		}

		public static object GetProperty(object target, string name)
		{
			//Discarded unreachable code: IL_0022, IL_0047
			if (!Initialized)
			{
				throw new AssertionFailedException("Initialized");
			}
			try
			{
				return RuntimeServices.GetProperty(target, name);
			}
			catch (MissingMemberException)
			{
				if (target.GetType().IsValueType)
				{
					throw;
				}
				return ExpandoServices.GetExpandoProperty(target, name);
			}
		}

		public static object SetProperty(object target, string name, object value)
		{
			//Discarded unreachable code: IL_0023, IL_0049
			if (!Initialized)
			{
				throw new AssertionFailedException("Initialized");
			}
			try
			{
				return RuntimeServices.SetProperty(target, name, value);
			}
			catch (MissingMemberException)
			{
				if (target.GetType().IsValueType)
				{
					throw;
				}
				return ExpandoServices.SetExpandoProperty(target, name, value);
			}
		}

		public static Type GetTypeOf(object o)
		{
			return (o != null) ? o.GetType() : null;
		}

		public static bool IsGenerator(object obj)
		{
			Type type = obj.GetType();
			return type == EnumeratorType || EnumeratorType.IsAssignableFrom(type) || typeof(AbstractGenerator).IsAssignableFrom(type);
		}

		public static bool IsStaticMethod(Type type, string name, object[] args)
		{
			//Discarded unreachable code: IL_0012, IL_001f
			try
			{
				return type.GetMethod(name).IsStatic;
			}
			catch (Exception)
			{
				return true;
			}
		}

		public static IEnumerator GetEnumerator(object obj)
		{
			object result;
			if (obj == null)
			{
				result = EmptyEnumerator;
			}
			else if (IsValueTypeArray(obj) || obj is Array)
			{
				object obj2 = obj;
				if (!(obj2 is IList))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(IList));
				}
				result = new ListUpdateableEnumerator((IList)obj2);
			}
			else
			{
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					result = enumerable.GetEnumerator();
				}
				else
				{
					IEnumerator enumerator = obj as IEnumerator;
					result = ((enumerator == null) ? RuntimeServices.GetEnumerable(obj).GetEnumerator() : enumerator);
				}
			}
			return (IEnumerator)result;
		}

		public static void Update(IEnumerator e, object newValue)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e is ListUpdateableEnumerator)
			{
				((ListUpdateableEnumerator)e).Update(newValue);
			}
		}

		public static bool IsValueTypeArray(object obj)
		{
			return obj is System.Array && obj.GetType().GetElementType().IsValueType;
		}

		public static void PropagateValueTypeChanges(ValueTypeChange[] changes)
		{
			int i = 0;
			for (int length = changes.Length; i < length && changes[i].Propagate(); i = checked(i + 1))
			{
			}
		}

		private static void _0024static_initializer_0024()
		{
			EmptyEnumerator = new object[0].GetEnumerator();
			EnumeratorType = typeof(IEnumerator);
		}
	}
}
