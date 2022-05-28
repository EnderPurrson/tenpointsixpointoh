using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Prime31
{
	public class DTOBase
	{
		[CompilerGenerated]
		private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey0
		{
			internal FieldInfo theInfo;

			internal DTOBase _0024this;

			internal void _003C_003Em__0(object val)
			{
				theInfo.SetValue((object)_0024this, val);
			}
		}

		[CompilerGenerated]
		private sealed class _003CgetMembersWithSetters_003Ec__AnonStorey1
		{
			internal PropertyInfo theInfo;

			internal DTOBase _0024this;

			internal void _003C_003Em__0(object val)
			{
				theInfo.SetValue((object)_0024this, val, (object[])default(object[]));
			}
		}

		public static List<T> listFromJson<T>(string json) where T : DTOBase
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			List<object> val = json.listFromJson();
			List<T> val2 = new List<T>();
			Enumerator<object> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					T val3 = Activator.CreateInstance<T>();
					val3.setDataFromDictionary(current as Dictionary<string, object>);
					val2.Add(val3);
				}
				return val2;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public void setDataFromJson(string json)
		{
			setDataFromDictionary(json.dictionaryFromJson());
		}

		public void setDataFromDictionary(Dictionary<string, object> dict)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, Action<object>> membersWithSetters = getMembersWithSetters();
			Enumerator<string, object> enumerator = dict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if (membersWithSetters.ContainsKey(current.get_Key()))
					{
						try
						{
							membersWithSetters.get_Item(current.get_Key()).Invoke(current.get_Value());
						}
						catch (global::System.Exception obj)
						{
							Utils.logObject(obj);
						}
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		private bool shouldIncludeTypeWithSetters(global::System.Type type)
		{
			if (type.get_IsGenericType())
			{
				return false;
			}
			if (type.get_Namespace().StartsWith("System"))
			{
				return true;
			}
			return false;
		}

		protected Dictionary<string, Action<object>> getMembersWithSetters()
		{
			Dictionary<string, Action<object>> val = new Dictionary<string, Action<object>>();
			FieldInfo[] fields = base.GetType().GetFields();
			foreach (FieldInfo val2 in fields)
			{
				_003CgetMembersWithSetters_003Ec__AnonStorey0 _003CgetMembersWithSetters_003Ec__AnonStorey = new _003CgetMembersWithSetters_003Ec__AnonStorey0();
				_003CgetMembersWithSetters_003Ec__AnonStorey._0024this = this;
				if (shouldIncludeTypeWithSetters(val2.get_FieldType()))
				{
					_003CgetMembersWithSetters_003Ec__AnonStorey.theInfo = val2;
					val.set_Item(((MemberInfo)val2).get_Name(), (Action<object>)_003CgetMembersWithSetters_003Ec__AnonStorey._003C_003Em__0);
				}
			}
			PropertyInfo[] properties = base.GetType().GetProperties();
			foreach (PropertyInfo val3 in properties)
			{
				if (shouldIncludeTypeWithSetters(val3.get_PropertyType()) && val3.get_CanWrite() && val3.GetSetMethod() != null)
				{
					_003CgetMembersWithSetters_003Ec__AnonStorey1 _003CgetMembersWithSetters_003Ec__AnonStorey2 = new _003CgetMembersWithSetters_003Ec__AnonStorey1();
					_003CgetMembersWithSetters_003Ec__AnonStorey2._0024this = this;
					_003CgetMembersWithSetters_003Ec__AnonStorey2.theInfo = val3;
					val.set_Item(((MemberInfo)val3).get_Name(), (Action<object>)_003CgetMembersWithSetters_003Ec__AnonStorey2._003C_003Em__0);
				}
			}
			return val;
		}

		public override string ToString()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			StringBuilder val = new StringBuilder();
			val.AppendFormat("[{0}]:", (object)base.GetType());
			FieldInfo[] fields = base.GetType().GetFields();
			foreach (FieldInfo val2 in fields)
			{
				val.AppendFormat(", {0}: {1}", (object)((MemberInfo)val2).get_Name(), val2.GetValue((object)this));
			}
			PropertyInfo[] properties = base.GetType().GetProperties();
			foreach (PropertyInfo val3 in properties)
			{
				val.AppendFormat(", {0}: {1}", (object)((MemberInfo)val3).get_Name(), val3.GetValue((object)this, (object[])default(object[])));
			}
			return ((object)val).ToString();
		}
	}
}
