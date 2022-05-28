using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated.CustomEvent
{
	internal sealed class CustomEvent : AggregatedEvent
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2
		{
			public CustomEventData a;

			public bool _003CAddEvent_003Eb__0(CustomEventData x)
			{
				return x.CreationTimestamp == a.CreationTimestamp;
			}
		}

		private static readonly string NAME = "name";

		private static readonly string ENTRIES = "entries";

		private static readonly string START_EVENT = "t1";

		private static readonly string END_EVENT = "t2";

		private static readonly string DURATION = "d";

		private static readonly string PARAMS_ENUMERATION = "p";

		private static readonly string DOUBLE = "double";

		private static readonly string STRING = "string";

		private static readonly string DATE = "date";

		private CustomEventData customEventData;

		public Dictionary<string, List<CustomEventData>> CustomEventsData
		{
			[CompilerGenerated]
			get
			{
				return _003CCustomEventsData_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCustomEventsData_003Ek__BackingField = value;
			}
		}

		public Dictionary<string, CustomEventData> NotFinishedEvents
		{
			[CompilerGenerated]
			get
			{
				return _003CNotFinishedEvents_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CNotFinishedEvents_003Ek__BackingField = value;
			}
		}

		public CustomEvent()
		{
		}

		public CustomEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				CustomEventsData = info.GetValue("CustomEventsData", typeof(Dictionary<string, List<CustomEventData>>)) as Dictionary<string, List<CustomEventData>>;
				NotFinishedEvents = info.GetValue("NotFinishedEvents", typeof(Dictionary<string, CustomEventData>)) as Dictionary<string, CustomEventData>;
				customEventData = info.GetValue("customEventData", typeof(CustomEventData)) as CustomEventData;
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
				info.AddValue("CustomEventsData", CustomEventsData);
				info.AddValue("NotFinishedEvents", NotFinishedEvents);
				info.AddValue("customEventData", customEventData);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public CustomEvent(CustomEventData customEventData)
			: base(EventType.CustomEvent)
		{
			this.customEventData = customEventData;
			SelfAggregate();
		}

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			JSONArray jSONArray = new JSONArray();
			Enumerator<string, List<CustomEventData>> enumerator = CustomEventsData.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<CustomEventData>> current = enumerator.get_Current();
					if (current.get_Key() == null || current.get_Value() == null)
					{
						continue;
					}
					JSONClass jSONClass = new JSONClass();
					jSONArray.Add(jSONClass);
					jSONClass.Add(NAME, Uri.EscapeDataString(current.get_Key()));
					JSONArray jSONArray2 = new JSONArray();
					List<CustomEventData> value = current.get_Value();
					jSONClass.Add(ENTRIES, jSONArray2);
					Enumerator<CustomEventData> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							CustomEventData current2 = enumerator2.get_Current();
							if (current2 == null)
							{
								continue;
							}
							JSONClass jSONClass2 = new JSONClass();
							jSONArray2.Add(jSONClass2);
							AddPendingToJSON(jSONClass2, current2.PendingData);
							jSONClass2.Add(START_EVENT, new JSONData(current2.StartTime));
							if (!current2.HasParams || current2.Params == null)
							{
								continue;
							}
							JSONClass jSONClass3 = new JSONClass();
							jSONClass2.Add(PARAMS_ENUMERATION, jSONClass3);
							JSONClass jSONClass4 = new JSONClass();
							jSONClass3.Add(START_EVENT, jSONClass4);
							if (current2.Params.HasNumeric)
							{
								JSONClass jSONClass5 = new JSONClass();
								jSONClass4.Add(DOUBLE, jSONClass5);
								Enumerator<string, double> enumerator3 = current2.Params.DoubleParams.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										KeyValuePair<string, double> current3 = enumerator3.get_Current();
										jSONClass5.Add(Uri.EscapeDataString(current3.get_Key()), new JSONData(current3.get_Value()));
									}
								}
								finally
								{
									((global::System.IDisposable)enumerator3).Dispose();
								}
								Enumerator<string, long> enumerator4 = current2.Params.LongParams.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										KeyValuePair<string, long> current4 = enumerator4.get_Current();
										jSONClass5.Add(Uri.EscapeDataString(current4.get_Key()), new JSONData(current4.get_Value()));
									}
								}
								finally
								{
									((global::System.IDisposable)enumerator4).Dispose();
								}
								Enumerator<string, int> enumerator5 = current2.Params.IntParams.GetEnumerator();
								try
								{
									while (enumerator5.MoveNext())
									{
										KeyValuePair<string, int> current5 = enumerator5.get_Current();
										jSONClass5.Add(Uri.EscapeDataString(current5.get_Key()), new JSONData(current5.get_Value()));
									}
								}
								finally
								{
									((global::System.IDisposable)enumerator5).Dispose();
								}
							}
							if (!current2.Params.HasStrings)
							{
								continue;
							}
							JSONClass jSONClass6 = new JSONClass();
							jSONClass4.Add(STRING, jSONClass6);
							Enumerator<string, string> enumerator6 = current2.Params.StringParams.GetEnumerator();
							try
							{
								while (enumerator6.MoveNext())
								{
									KeyValuePair<string, string> current6 = enumerator6.get_Current();
									jSONClass6.Add(Uri.EscapeDataString(current6.get_Key()), new JSONData(Uri.EscapeDataString(current6.get_Value())));
								}
							}
							finally
							{
								((global::System.IDisposable)enumerator6).Dispose();
							}
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

		public override void AddEvent(AggregatedEvent metric)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, List<CustomEventData>> enumerator = (metric as CustomEvent).CustomEventsData.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<CustomEventData>> current = enumerator.get_Current();
					if (!CustomEventsData.ContainsKey(current.get_Key()))
					{
						continue;
					}
					List<CustomEventData> val = CustomEventsData.get_Item(current.get_Key());
					Enumerator<CustomEventData> enumerator2 = current.get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Predicate<CustomEventData> val2 = null;
							_003C_003Ec__DisplayClass2 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass2();
							_003C_003Ec__DisplayClass.a = enumerator2.get_Current();
							if (val2 == null)
							{
								val2 = _003C_003Ec__DisplayClass._003CAddEvent_003Eb__0;
							}
							CustomEventData customEventData = val.Find(val2);
							if (customEventData != null)
							{
								val.Remove(customEventData);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (metric != null)
			{
				AddSingleData(metric as CustomEvent);
			}
		}

		protected override void SelfAggregate()
		{
			CustomEventsData = new Dictionary<string, List<CustomEventData>>();
			NotFinishedEvents = new Dictionary<string, CustomEventData>();
			if (customEventData != null)
			{
				if (customEventData.Type != 0)
				{
					NotFinishedEvents.Add(customEventData.EventId, customEventData);
					return;
				}
				Dictionary<string, List<CustomEventData>> customEventsData = CustomEventsData;
				string eventName = customEventData.EventName;
				List<CustomEventData> val = new List<CustomEventData>();
				val.Add(customEventData);
				customEventsData.Add(eventName, val);
			}
		}

		public override void AddPendingEvents(List<string> events)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, List<CustomEventData>> enumerator = CustomEventsData.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Enumerator<CustomEventData> enumerator2 = enumerator.get_Current().get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							CustomEventData current = enumerator2.get_Current();
							current.PendingData = events;
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public override bool IsReadyToSend()
		{
			return CustomEventsData.get_Count() > 0;
		}

		public override bool IsNeedToClear()
		{
			return NotFinishedEvents.get_Count() == 0;
		}

		private void AddSingleData(CustomEvent ce)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, List<CustomEventData>> customEventsData = ce.CustomEventsData;
			Enumerator<string, List<CustomEventData>> enumerator = customEventsData.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<CustomEventData>> current = enumerator.get_Current();
					if (CustomEventsData.ContainsKey(current.get_Key()))
					{
						CustomEventsData.get_Item(current.get_Key()).AddRange((global::System.Collections.Generic.IEnumerable<CustomEventData>)current.get_Value());
					}
					else
					{
						CustomEventsData.Add(current.get_Key(), current.get_Value());
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		private void FinishEvent(CustomEventData complete)
		{
			if (CustomEventsData.ContainsKey(complete.EventName))
			{
				CustomEventsData.get_Item(complete.EventName).Add(complete);
			}
			else
			{
				Dictionary<string, List<CustomEventData>> customEventsData = CustomEventsData;
				string eventName = complete.EventName;
				List<CustomEventData> val = new List<CustomEventData>();
				val.Add(complete);
				customEventsData.Add(eventName, val);
			}
			NotFinishedEvents.Remove(complete.EventId);
		}

		public override bool IsEqualToMetric(Event other)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			CustomEvent customEvent = other as CustomEvent;
			if (customEventData == null || customEvent.customEventData == null)
			{
				return false;
			}
			return customEventData.EventName.Equals(customEvent.customEventData.EventName);
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
