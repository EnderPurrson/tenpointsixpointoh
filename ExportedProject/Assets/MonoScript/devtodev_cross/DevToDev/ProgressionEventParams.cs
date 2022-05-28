using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public class ProgressionEventParams : ISaveable
	{
		protected static readonly string SPENT_KEY = "spent";

		protected static readonly string EARNED_KEY = "earned";

		protected static readonly string PARAMS_KEY = "params";

		protected static readonly string SUCCESS_KEY = "success";

		internal string EventName
		{
			[CompilerGenerated]
			get
			{
				return _003CEventName_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventName_003Ek__BackingField = value;
			}
		}

		internal Dictionary<string, int> Spent
		{
			[CompilerGenerated]
			get
			{
				return _003CSpent_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSpent_003Ek__BackingField = value;
			}
		}

		internal Dictionary<string, int> Earned
		{
			[CompilerGenerated]
			get
			{
				return _003CEarned_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEarned_003Ek__BackingField = value;
			}
		}

		internal long EventStart
		{
			[CompilerGenerated]
			get
			{
				return _003CEventStart_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventStart_003Ek__BackingField = value;
			}
		}

		internal long EventFinish
		{
			[CompilerGenerated]
			get
			{
				return _003CEventFinish_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventFinish_003Ek__BackingField = value;
			}
		}

		internal bool? IsSuccessful
		{
			[CompilerGenerated]
			get
			{
				return _003CIsSuccessful_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIsSuccessful_003Ek__BackingField = value;
			}
		}

		public ProgressionEventParams()
		{
		}

		public ProgressionEventParams(ObjectInfo info)
		{
			try
			{
				EventName = info.GetValue("EventName", typeof(string)) as string;
				Spent = info.GetValue("Spent", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				Earned = info.GetValue("Earned", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				EventStart = (long)info.GetValue("EventStart", typeof(long));
				EventFinish = (long)info.GetValue("EventFinish", typeof(long));
				IsSuccessful = info.GetValue("IsSuccessful", typeof(bool?)) as bool?;
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("EventName", EventName);
				info.AddValue("Spent", Spent);
				info.AddValue("Earned", Earned);
				info.AddValue("EventStart", EventStart);
				info.AddValue("EventFinish", EventFinish);
				info.AddValue("IsSuccessful", IsSuccessful);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		internal ProgressionEventParams(ProgressionEventParams peparams)
		{
			Spent = peparams.Spent;
			Earned = peparams.Earned;
			IsSuccessful = peparams.IsSuccessful;
		}

		internal void SetStartTime(long time)
		{
			EventStart = time;
		}

		internal bool IsCorrect()
		{
			if (EventFinish != 0)
			{
				return EventStart != 0;
			}
			return false;
		}

		internal long GetFinishTime()
		{
			return EventFinish;
		}

		internal void SetFinishTime(long finishTime)
		{
			EventFinish = finishTime;
		}

		internal virtual void Merge(ProgressionEventParams value)
		{
			EventFinish = value.EventFinish;
			if (value.Earned != null)
			{
				Earned = value.Earned;
			}
			if (value.Spent != null)
			{
				Spent = value.Spent;
			}
			if (value.IsSuccessful.get_HasValue())
			{
				IsSuccessful = value.IsSuccessful;
			}
		}

		public void SetSuccessfulCompletion(bool success)
		{
			IsSuccessful = success;
		}

		internal void SetEventName(string eventName)
		{
			EventName = eventName;
		}

		internal string GetEventName()
		{
			return EventName;
		}

		public void SetEarned(Dictionary<string, int> earned)
		{
			if (earned != null)
			{
				Earned = earned;
			}
		}

		public void SetSpent(Dictionary<string, int> spent)
		{
			if (spent != null)
			{
				Spent = spent;
			}
		}

		internal virtual JSONNode ToJson()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			JSONNode jSONNode = new JSONClass();
			if (Spent != null && Spent.get_Count() > 0)
			{
				JSONNode jSONNode2 = new JSONClass();
				Enumerator<string, int> enumerator = Spent.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, int> current = enumerator.get_Current();
						jSONNode2.Add(Uri.EscapeDataString(current.get_Key()), new JSONData(current.get_Value()));
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				jSONNode.Add(SPENT_KEY, jSONNode2);
			}
			if (Earned != null && Earned.get_Count() > 0)
			{
				JSONNode jSONNode3 = new JSONClass();
				Enumerator<string, int> enumerator2 = Earned.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, int> current2 = enumerator2.get_Current();
						jSONNode3.Add(Uri.EscapeDataString(current2.get_Key()), new JSONData(current2.get_Value()));
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator2).Dispose();
				}
				jSONNode.Add(EARNED_KEY, jSONNode3);
			}
			JSONNode jSONNode4 = new JSONClass();
			if (IsSuccessful.get_HasValue())
			{
				jSONNode4.Add(SUCCESS_KEY, new JSONData(IsSuccessful));
			}
			else
			{
				jSONNode4.Add(SUCCESS_KEY, new JSONData(false));
			}
			jSONNode.Add(PARAMS_KEY, jSONNode4);
			return jSONNode;
		}

		internal virtual ProgressionEventParams Clone()
		{
			ProgressionEventParams progressionEventParams = new ProgressionEventParams();
			progressionEventParams.Earned = Earned;
			progressionEventParams.Spent = Spent;
			progressionEventParams.EventName = EventName;
			progressionEventParams.EventStart = EventStart;
			progressionEventParams.EventFinish = EventFinish;
			progressionEventParams.IsSuccessful = IsSuccessful;
			return progressionEventParams;
		}
	}
}
