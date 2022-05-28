using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated
{
	internal abstract class AggregatedEvent : Event
	{
		public AggregatedEvent()
		{
		}

		protected AggregatedEvent(EventType type)
			: base(type)
		{
		}

		public AggregatedEvent(ObjectInfo info)
			: base(info)
		{
		}

		public abstract void AddEvent(AggregatedEvent metric);

		protected abstract void SelfAggregate();

		public abstract bool IsReadyToSend();

		public abstract bool IsNeedToClear();

		public abstract void RemoveSentMetrics();

		public void AddPendingToJSON(JSONNode json, List<string> pending)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (pending == null || pending.get_Count() <= 0)
			{
				return;
			}
			JSONArray jSONArray = new JSONArray();
			Enumerator<string> enumerator = pending.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					jSONArray.Add(Uri.EscapeDataString(current));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (json[Event.IN_PROGRESS] == null)
			{
				json.Add(Event.IN_PROGRESS, jSONArray);
			}
		}
	}
}
