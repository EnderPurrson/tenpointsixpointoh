using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Aggregated.CustomEvent
{
	internal class CustomEventData : ISaveable
	{
		public long CreationTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;

		public string EventId
		{
			[CompilerGenerated]
			get
			{
				return _003CEventId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEventId_003Ek__BackingField = value;
			}
		}

		public string EventName
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

		public long StartTime
		{
			[CompilerGenerated]
			get
			{
				return _003CStartTime_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CStartTime_003Ek__BackingField = value;
			}
		}

		public long EndTime
		{
			[CompilerGenerated]
			get
			{
				return _003CEndTime_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEndTime_003Ek__BackingField = value;
			}
		}

		public long Duration
		{
			[CompilerGenerated]
			get
			{
				return _003CDuration_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDuration_003Ek__BackingField = value;
			}
		}

		public CustomEventParams Params
		{
			[CompilerGenerated]
			get
			{
				return _003CParams_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CParams_003Ek__BackingField = value;
			}
		}

		public CustomEventDataType Type
		{
			[CompilerGenerated]
			get
			{
				return _003CType_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CType_003Ek__BackingField = value;
			}
		}

		public List<string> PendingData
		{
			[CompilerGenerated]
			get
			{
				return _003CPendingData_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPendingData_003Ek__BackingField = value;
			}
		}

		public bool HasParams
		{
			get
			{
				if (Params != null)
				{
					return Params.Count > 0;
				}
				return false;
			}
		}

		private CustomEventData()
		{
			Duration = 0L;
			StartTime = DeviceHelper.Instance.GetUnixTime() / 1000;
			Params = new CustomEventParams();
		}

		public CustomEventData(ObjectInfo info)
		{
			try
			{
				EventId = info.GetValue("EventId", typeof(string)) as string;
				EventName = info.GetValue("EventName", typeof(string)) as string;
				StartTime = (long)info.GetValue("StartTime", typeof(long));
				EndTime = (long)info.GetValue("EndTime", typeof(long));
				Duration = (long)info.GetValue("Duration", typeof(long));
				Params = info.GetValue("ceParams", typeof(CustomEventParams)) as CustomEventParams;
				Type = (CustomEventDataType)(int)info.GetValue("Type", typeof(int));
				PendingData = info.GetValue("pendingData", typeof(List<string>)) as List<string>;
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
				info.AddValue("EventId", EventId);
				info.AddValue("EventName", EventName);
				info.AddValue("StartTime", StartTime);
				info.AddValue("EndTime", EndTime);
				info.AddValue("Duration", Duration);
				info.AddValue("Type", (int)Type);
				info.AddValue("ceParams", Params);
				info.AddValue("pendingData", PendingData);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public static CustomEventData SingleData(string eventName, CustomEventParams eventParams)
		{
			CustomEventData customEventData = new CustomEventData();
			customEventData.Type = CustomEventDataType.Single;
			customEventData.EventName = eventName;
			if (eventParams != null)
			{
				customEventData.Params.CopyFromAnother(eventParams);
			}
			return customEventData;
		}
	}
}
