using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics.Simple;
using DevToDev.Logic;

namespace DevToDev.Data.Metrics.Specific
{
	internal class PeopleEvent : SimpleEvent
	{
		public bool IsRemoved
		{
			[CompilerGenerated]
			get
			{
				return _003CIsRemoved_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIsRemoved_003Ek__BackingField = value;
			}
		}

		public PeopleEvent()
			: base(EventType.UserCard)
		{
			IsRemoved = false;
			parameters.Remove(Event.TIMESTAMP);
		}

		public PeopleEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				IsRemoved = (bool)info.GetValue("isRemoved", typeof(bool));
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("isRemoved", IsRemoved);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public bool AddOrReplace(string key, object value)
		{
			if (parameters.ContainsKey(key))
			{
				if (parameters.get_Item(key) == null)
				{
					if (value is Gender)
					{
						parameters.set_Item(key, (object)(int)(Gender)value);
					}
					else
					{
						parameters.set_Item(key, value);
					}
					return true;
				}
				if (!parameters.get_Item(key).Equals(value))
				{
					if (value is Gender)
					{
						parameters.set_Item(key, (object)(int)(Gender)value);
					}
					else
					{
						parameters.set_Item(key, value);
					}
					return true;
				}
				return false;
			}
			if (value is Gender)
			{
				parameters.Add(key, (object)(int)(Gender)value);
			}
			else
			{
				parameters.Add(key, value);
			}
			return true;
		}

		public void ClearParameters()
		{
			parameters.Clear();
		}

		private JSONArray ToJsonArray(List<object> value)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			JSONArray jSONArray = new JSONArray();
			Enumerator<object> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					if (current is global::System.Collections.IList)
					{
						jSONArray.Add(ToJsonArray(current as List<object>));
					}
					else if (current is string)
					{
						jSONArray.Add(new JSONData(Uri.EscapeDataString(current.ToString())));
					}
					else
					{
						jSONArray.Add(new JSONData(current));
					}
				}
				return jSONArray;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public bool NeedToSend(List<string> excluded)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			Enumerator<string, object> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if ((excluded == null || !excluded.Contains(current.get_Key())) && !current.get_Key().Equals(Event.IN_PROGRESS))
					{
						num++;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (num <= 0)
			{
				return IsRemoved;
			}
			return true;
		}

		public Dictionary<string, object> Merge(PeopleEvent peopleEvent, bool ageGenderAndCheaterOnly)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, object> val = new Dictionary<string, object>();
			Enumerator<string, object> enumerator = peopleEvent.parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if (!ageGenderAndCheaterOnly)
					{
						if (!parameters.ContainsKey(current.get_Key()))
						{
							parameters.Add(current.get_Key(), current.get_Value());
						}
						val.Add(current.get_Key(), current.get_Value());
					}
					else if (current.get_Key().Equals(PeopleLogic.CHEATER_KEY))
					{
						if (!parameters.ContainsKey(current.get_Key()))
						{
							parameters.Add(current.get_Key(), current.get_Value());
						}
						val.Add(current.get_Key(), current.get_Value());
					}
				}
				return val;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			List<string> excludedUserData = SDKClient.Instance.NetworkStorage.ExcludedUserData;
			JSONArray jSONArray = null;
			if (IsRemoved)
			{
				JSONClass jSONClass = new JSONClass();
				jSONClass.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
				object aData = null;
				jSONClass.Add(PeopleLogic.DATA_KEY, new JSONData(aData));
				IsRemoved = false;
				AddPendingToJSON(jSONClass);
				if (!NeedToSend(excludedUserData))
				{
					return jSONClass;
				}
				jSONArray = new JSONArray();
				jSONArray.Add(jSONClass);
			}
			JSONClass jSONClass2 = new JSONClass();
			jSONClass2.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
			JSONClass jSONClass3 = new JSONClass();
			jSONClass2.Add(PeopleLogic.DATA_KEY, jSONClass3);
			Enumerator<string, object> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if (!current.get_Key().Equals(Event.IN_PROGRESS))
					{
						if (excludedUserData == null || !excludedUserData.Contains(current.get_Key()))
						{
							if (current.get_Value() is global::System.Collections.IList)
							{
								jSONClass3.Add(Uri.EscapeDataString(current.get_Key()), ToJsonArray(current.get_Value() as List<object>));
							}
							else if (current.get_Value() is string)
							{
								jSONClass3.Add(Uri.EscapeDataString(current.get_Key()), new JSONData(Uri.EscapeDataString(current.get_Value().ToString())));
							}
							else
							{
								jSONClass3.Add(Uri.EscapeDataString(current.get_Key()), new JSONData(current.get_Value()));
							}
						}
					}
					else
					{
						AddPendingToJSON(jSONClass2);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (jSONArray != null)
			{
				jSONArray.Add(jSONClass2);
				return jSONArray;
			}
			return jSONClass2;
		}
	}
}
