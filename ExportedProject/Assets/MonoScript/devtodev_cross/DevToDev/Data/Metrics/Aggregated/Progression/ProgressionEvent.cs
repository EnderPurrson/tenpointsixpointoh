using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Aggregated.Progression
{
	internal class ProgressionEvent : AggregatedEvent
	{
		protected static readonly string ID = "id";

		private Dictionary<string, ProgressionEventParams> events;

		internal Dictionary<string, List<ProgressionEventParams>> CompletedEvents
		{
			[CompilerGenerated]
			get
			{
				return _003CCompletedEvents_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCompletedEvents_003Ek__BackingField = value;
			}
		}

		private Dictionary<string, ProgressionEventParams> Events
		{
			get
			{
				if (events == null)
				{
					events = new Dictionary<string, ProgressionEventParams>();
				}
				return events;
			}
		}

		public ProgressionEvent(ProgressionEvent metric)
			: base(EventType.ProgressionEvent)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			CompletedEvents = new Dictionary<string, List<ProgressionEventParams>>();
			Enumerator<string, ProgressionEventParams> enumerator = metric.Events.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, ProgressionEventParams> current = enumerator.get_Current();
					Events.Add(current.get_Key(), current.get_Value().Clone());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			Enumerator<string, List<ProgressionEventParams>> enumerator2 = metric.CompletedEvents.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, List<ProgressionEventParams>> current2 = enumerator2.get_Current();
					CompletedEvents.Add(current2.get_Key(), new List<ProgressionEventParams>());
					Enumerator<ProgressionEventParams> enumerator3 = current2.get_Value().GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							ProgressionEventParams current3 = enumerator3.get_Current();
							CompletedEvents.get_Item(current2.get_Key()).Add(current3.Clone());
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator3).Dispose();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			SelfAggregate();
		}

		public ProgressionEvent(ProgressionEventParams eventData)
			: base(EventType.ProgressionEvent)
		{
			CompletedEvents = new Dictionary<string, List<ProgressionEventParams>>();
			Events.Add(eventData.GetEventName(), eventData);
			if (eventData.GetFinishTime() == 0)
			{
				Log.D("Progression event {0} is started.", eventData.GetEventName());
			}
			SelfAggregate();
		}

		public ProgressionEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				CompletedEvents = info.GetValue("CompletedEvents", typeof(Dictionary<string, List<ProgressionEventParams>>)) as Dictionary<string, List<ProgressionEventParams>>;
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
				info.AddValue("CompletedEvents", CompletedEvents);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		private void AddOrMergeParams(string paramName, ProgressionEventParams destinationParam, ProgressionEventParams sourceParam)
		{
			if (destinationParam.GetFinishTime() == 0 && sourceParam.GetFinishTime() != 0)
			{
				destinationParam.Merge(sourceParam);
				Events.Remove(paramName);
				AddFinishedEvent(paramName, destinationParam);
				return;
			}
			if (destinationParam.GetFinishTime() == 0)
			{
				Events.set_Item(paramName, sourceParam);
				return;
			}
			Events.Remove(paramName);
			if (sourceParam.GetFinishTime() != 0)
			{
				AddFinishedEvent(paramName, sourceParam);
			}
			else
			{
				Events.set_Item(paramName, sourceParam);
			}
		}

		public override void AddEvent(AggregatedEvent metric)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			ProgressionEvent progressionEvent = metric as ProgressionEvent;
			Enumerator<string, List<ProgressionEventParams>> enumerator = progressionEvent.CompletedEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<ProgressionEventParams>> current = enumerator.get_Current();
					CompletedEvents.Add(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			Enumerator<string, ProgressionEventParams> enumerator2 = progressionEvent.Events.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, ProgressionEventParams> current2 = enumerator2.get_Current();
					if (Events.ContainsKey(current2.get_Key()))
					{
						ProgressionEventParams progressionEventParams = Events.get_Item(current2.get_Key());
						if (!object.Equals((object)((object)progressionEventParams).GetType(), (object)((object)current2.get_Value()).GetType()))
						{
							Events.set_Item(current2.get_Key(), current2.get_Value());
						}
						else
						{
							AddOrMergeParams(current2.get_Key(), progressionEventParams, current2.get_Value());
						}
					}
					else if (current2.get_Value().GetFinishTime() == 0)
					{
						Dictionary<string, ProgressionEventParams> val = new Dictionary<string, ProgressionEventParams>();
						Enumerator<string, ProgressionEventParams> enumerator3 = Events.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								KeyValuePair<string, ProgressionEventParams> current3 = enumerator3.get_Current();
								if (!object.Equals((object)((object)current3.get_Value()).GetType(), (object)((object)current2.get_Value()).GetType()))
								{
									val.Add(current3.get_Key(), current3.get_Value());
								}
							}
						}
						finally
						{
							((global::System.IDisposable)enumerator3).Dispose();
						}
						events = val;
						Events.Add(current2.get_Key(), current2.get_Value());
					}
					else
					{
						AddFinishedEvent(current2.get_Key(), current2.get_Value());
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
		}

		private void AddFinishedEvent(string name, ProgressionEventParams eventParam)
		{
			if (eventParam.IsCorrect())
			{
				if (!CompletedEvents.ContainsKey(name))
				{
					CompletedEvents.Add(name, new List<ProgressionEventParams>());
				}
				CompletedEvents.get_Item(name).Add(eventParam);
				Log.R("Progression event {0} is finished.", name);
				Log.R("Metric {0} added to storage", base.MetricName);
			}
			else
			{
				Log.R("Progression event {0} could not be finished. There is no active progression event {0}.", name);
			}
		}

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (!IsReadyToSend())
			{
				return null;
			}
			JSONArray jSONArray = new JSONArray();
			Enumerator<string, List<ProgressionEventParams>> enumerator = CompletedEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<ProgressionEventParams>> current = enumerator.get_Current();
					if (current.get_Key() == null || current.get_Value() == null)
					{
						continue;
					}
					Enumerator<ProgressionEventParams> enumerator2 = current.get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							ProgressionEventParams current2 = enumerator2.get_Current();
							JSONNode jSONNode = current2.ToJson();
							jSONNode.Add(ID, Uri.EscapeDataString(current.get_Key()));
							jSONNode.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
							jSONArray.Add(jSONNode);
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2).Dispose();
					}
				}
				return jSONArray;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public List<string> GetPendingEvents()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			List<string> val = new List<string>();
			Enumerator<string, ProgressionEventParams> enumerator = Events.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, ProgressionEventParams> current = enumerator.get_Current();
					if (current.get_Value().GetFinishTime() == 0)
					{
						val.Add(current.get_Value().EventName);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (val.get_Count() > 0)
			{
				return val;
			}
			return null;
		}

		public override bool IsNeedToClear()
		{
			return Events.get_Count() == 0;
		}

		public override bool IsReadyToSend()
		{
			return CompletedEvents.get_Count() > 0;
		}

		protected override void SelfAggregate()
		{
		}

		public override void RemoveSentMetrics()
		{
			CompletedEvents.Clear();
		}

		public void ClearUnfinishedEvents()
		{
			Events.Clear();
		}
	}
}
