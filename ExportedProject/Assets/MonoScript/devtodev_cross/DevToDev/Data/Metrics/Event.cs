using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics
{
	internal abstract class Event : ISaveable
	{
		public static readonly string TIMESTAMP = "timestamp";

		public static readonly string IN_PROGRESS = "inProgress";

		protected EventType metricType;

		protected string metricName;

		protected string metricCode;

		protected Dictionary<string, object> parameters;

		public EventType MetricType
		{
			get
			{
				return metricType;
			}
			set
			{
				metricType = value;
			}
		}

		public string MetricName
		{
			get
			{
				return metricName;
			}
			set
			{
				metricName = value;
			}
		}

		public string MetricCode
		{
			get
			{
				return metricCode;
			}
			set
			{
				metricCode = value;
			}
		}

		public Dictionary<string, object> Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				parameters = value;
			}
		}

		public Event()
		{
		}

		protected Event(EventType type)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			metricType = type;
			metricName = EventConst.EventsInfo.get_Item(type).get_Key();
			metricCode = EventConst.EventsInfo.get_Item(type).get_Value();
			parameters = new Dictionary<string, object>();
			parameters.Add(TIMESTAMP, (object)(DeviceHelper.Instance.GetUnixTime() / 1000));
		}

		public Event(ObjectInfo info)
		{
			try
			{
				metricType = (EventType)(int)info.GetValue("metricType", typeof(int));
				metricName = info.GetValue("metricName", typeof(string)) as string;
				metricCode = info.GetValue("metricCode", typeof(string)) as string;
				parameters = info.GetValue("parameters", typeof(Dictionary<string, object>)) as Dictionary<string, object>;
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
				info.AddValue("metricType", (int)metricType);
				info.AddValue("metricName", metricName);
				info.AddValue("metricCode", metricCode);
				info.AddValue("parameters", parameters);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public abstract JSONNode GetAdditionalDataJson();

		protected bool IsMetricTypeEqual(Event other)
		{
			if (metricCode != other.metricCode)
			{
				return false;
			}
			return ((object)other).GetType().Equals(((object)this).GetType());
		}

		protected bool IsParameterEqual(Event other, string paramName)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			if (!parameters.ContainsKey(paramName))
			{
				return false;
			}
			if (!parameters.ContainsKey(paramName))
			{
				return false;
			}
			return parameters.get_Item(paramName).Equals(other.parameters.get_Item(paramName));
		}

		public virtual void AddPendingEvents(List<string> events)
		{
			if (!parameters.ContainsKey(IN_PROGRESS))
			{
				parameters.Add(IN_PROGRESS, (object)events);
			}
			else
			{
				parameters.set_Item(IN_PROGRESS, (object)events);
			}
		}

		public void AddPendingToJSON(JSONNode json)
		{
			JSONArray pendingEventsJson = GetPendingEventsJson();
			if (pendingEventsJson != null && json[IN_PROGRESS] == null)
			{
				json.Add(IN_PROGRESS, pendingEventsJson);
			}
		}

		public JSONArray GetPendingEventsJson()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (parameters.ContainsKey(IN_PROGRESS))
			{
				List<string> val = parameters.get_Item(IN_PROGRESS) as List<string>;
				if (val != null)
				{
					JSONArray jSONArray = new JSONArray();
					Enumerator<string> enumerator = val.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.get_Current();
							jSONArray.Add(Uri.EscapeDataString(current));
						}
						return jSONArray;
					}
					finally
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
			}
			return null;
		}

		public virtual bool IsEqualToMetric(Event other)
		{
			return false;
		}
	}
}
