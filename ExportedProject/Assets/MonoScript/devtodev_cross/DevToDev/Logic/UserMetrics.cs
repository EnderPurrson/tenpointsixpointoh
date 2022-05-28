using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Specific;

namespace DevToDev.Logic
{
	internal class UserMetrics : ISaveable
	{
		private static readonly string EVENTS = "events";

		private LevelData levelData;

		private Dictionary<string, List<Event>> simpleMetrics;

		private Dictionary<string, AggregatedEvent> aggregatedMetrics;

		public LevelData LevelData
		{
			get
			{
				return levelData;
			}
			internal set
			{
				if (value != null)
				{
					levelData = value;
				}
			}
		}

		public int Size
		{
			get
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				int num = 0;
				Enumerator<string, List<Event>> enumerator = simpleMetrics.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						num += enumerator.get_Current().get_Value().get_Count();
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				num += aggregatedMetrics.get_Count();
				return num + ((levelData != null) ? (levelData.IsNew ? 1 : 0) : 0);
			}
		}

		public UserMetrics()
		{
		}

		public UserMetrics(int level, bool isNew, Dictionary<string, int> resources)
		{
			levelData = new LevelData(level, isNew);
			levelData.Balance = resources;
			simpleMetrics = new Dictionary<string, List<Event>>();
			aggregatedMetrics = new Dictionary<string, AggregatedEvent>();
		}

		public UserMetrics(ObjectInfo info)
		{
			try
			{
				levelData = info.GetValue("levelData", typeof(LevelData)) as LevelData;
				simpleMetrics = info.GetValue("simpleMetrics", typeof(Dictionary<string, List<Event>>)) as Dictionary<string, List<Event>>;
				aggregatedMetrics = info.GetValue("aggregatedMetrics", typeof(Dictionary<string, AggregatedEvent>)) as Dictionary<string, AggregatedEvent>;
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
			if (aggregatedMetrics == null)
			{
				aggregatedMetrics = new Dictionary<string, AggregatedEvent>();
			}
			if (simpleMetrics == null)
			{
				simpleMetrics = new Dictionary<string, List<Event>>();
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("levelData", levelData);
				info.AddValue("simpleMetrics", simpleMetrics);
				info.AddValue("aggregatedMetrics", aggregatedMetrics);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public void ClearLevelData()
		{
			if (levelData != null)
			{
				levelData = new LevelData(levelData.Level, false);
			}
		}

		public bool isMetricExist(string metricCode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, List<Event>> enumerator = simpleMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().get_Key().ToUpper()
						.Equals(metricCode.ToUpper()))
					{
						return true;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			Enumerator<string, AggregatedEvent> enumerator2 = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.get_Current().get_Key().ToUpper()
						.Equals(metricCode.ToUpper()))
					{
						return true;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			return false;
		}

		public void AddProgressionParams(Event metric)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, AggregatedEvent> enumerator = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, AggregatedEvent> current = enumerator.get_Current();
					if (current.get_Value() is ProgressionEvent)
					{
						List<string> pendingEvents = (current.get_Value() as ProgressionEvent).GetPendingEvents();
						if (pendingEvents != null)
						{
							metric.AddPendingEvents(pendingEvents);
						}
						break;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		internal void addMetric(Event metric)
		{
			AddProgressionParams(metric);
			if (metric is AggregatedEvent)
			{
				string metricCode = metric.MetricCode;
				AggregatedEvent aggregatedEvent = metric as AggregatedEvent;
				if (aggregatedMetrics.ContainsKey(metricCode))
				{
					aggregatedMetrics.get_Item(metricCode).AddEvent(aggregatedEvent);
				}
				else
				{
					aggregatedMetrics.Add(metricCode, aggregatedEvent);
				}
			}
			else
			{
				if (!simpleMetrics.ContainsKey(metric.MetricCode))
				{
					simpleMetrics.Add(metric.MetricCode, new List<Event>());
				}
				simpleMetrics.get_Item(metric.MetricCode).Add(metric);
			}
		}

		public void ClearUnfinishedProgressionEvent()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, AggregatedEvent> enumerator = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, AggregatedEvent> current = enumerator.get_Current();
					if (current.get_Value() is ProgressionEvent)
					{
						ProgressionEvent progressionEvent = current.get_Value() as ProgressionEvent;
						progressionEvent.ClearUnfinishedEvents();
						break;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public ProgressionEvent GetProgressionEvent()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			ProgressionEvent result = null;
			Enumerator<string, AggregatedEvent> enumerator = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, AggregatedEvent> current = enumerator.get_Current();
					if (current.get_Value() is ProgressionEvent && !current.get_Value().IsNeedToClear())
					{
						result = current.get_Value() as ProgressionEvent;
					}
				}
				return result;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public ProgressionEvent GetRemoveProgressionEvent()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, AggregatedEvent> val = new Dictionary<string, AggregatedEvent>();
			ProgressionEvent result = null;
			Enumerator<string, AggregatedEvent> enumerator = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, AggregatedEvent> current = enumerator.get_Current();
					if (current.get_Value() is ProgressionEvent)
					{
						if (!current.get_Value().IsNeedToClear())
						{
							result = current.get_Value() as ProgressionEvent;
						}
						else
						{
							val.Add(current.get_Key(), current.get_Value());
						}
					}
					else
					{
						val.Add(current.get_Key(), current.get_Value());
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			aggregatedMetrics = val;
			return result;
		}

		public void clearMetrics()
		{
			simpleMetrics.Clear();
			aggregatedMetrics.Clear();
		}

		public void upEarned(string purchaseCurrency, int purchasePrice)
		{
			levelData.upEarned(purchaseCurrency, purchasePrice);
		}

		public void upSpend(string purchaseCurrency, int purchasePrice)
		{
			levelData.upSend(purchaseCurrency, purchasePrice);
		}

		public void upBought(string purchaseCurrency, int purchasePrice)
		{
			levelData.upBought(purchaseCurrency, purchasePrice);
		}

		public JSONNode DataToSend(int maxLevel)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			JSONClass jSONClass = new JSONClass();
			JSONClass jSONClass2 = levelData.DataToSend;
			if (levelData.Level >= maxLevel)
			{
				jSONClass2 = null;
			}
			if (aggregatedMetrics.get_Count() == 0 && simpleMetrics.get_Count() == 0 && jSONClass2 == null)
			{
				return null;
			}
			JSONClass jSONClass3 = new JSONClass();
			if (aggregatedMetrics.get_Count() != 0 || simpleMetrics.get_Count() != 0)
			{
				jSONClass.Add(EVENTS, jSONClass3);
			}
			Enumerator<string, List<Event>> enumerator = simpleMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<Event>> current = enumerator.get_Current();
					JSONArray jSONArray = new JSONArray();
					jSONClass3.Add(current.get_Key(), jSONArray);
					Enumerator<Event> enumerator2 = current.get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Event current2 = enumerator2.get_Current();
							jSONArray.Add(current2.GetAdditionalDataJson());
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
			Enumerator<string, AggregatedEvent> enumerator3 = aggregatedMetrics.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<string, AggregatedEvent> current3 = enumerator3.get_Current();
					JSONNode additionalDataJson = current3.get_Value().GetAdditionalDataJson();
					if (additionalDataJson != null)
					{
						jSONClass3.Add(current3.get_Key(), additionalDataJson);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator3).Dispose();
			}
			if (jSONClass2 != null)
			{
				global::System.Collections.IEnumerator enumerator4 = jSONClass2.GetEnumerator();
				while (enumerator4.MoveNext() && enumerator4.get_Current() != null)
				{
					KeyValuePair<string, JSONNode> val = (KeyValuePair<string, JSONNode>)enumerator4.get_Current();
					jSONClass.Add(val.get_Key(), val.get_Value());
				}
			}
			if (jSONClass3.Count == 0)
			{
				jSONClass.Remove(EVENTS);
			}
			if (jSONClass3.Count == 0 && jSONClass2 == null)
			{
				return null;
			}
			return jSONClass;
		}

		public void RemoveAppMetricsByType(EventType type)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			string value = EventConst.EventsInfo.get_Item(type).get_Value();
			if (simpleMetrics != null && simpleMetrics.ContainsKey(value))
			{
				simpleMetrics.Remove(value);
			}
			if (aggregatedMetrics != null && aggregatedMetrics.ContainsKey(value))
			{
				aggregatedMetrics.Remove(value);
			}
		}

		public PeopleEvent GetRemovePeopleEvent()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			string value = EventConst.EventsInfo.get_Item(EventType.UserCard).get_Value();
			if (simpleMetrics != null && simpleMetrics.ContainsKey(value))
			{
				List<Event> val = simpleMetrics.get_Item(value);
				simpleMetrics.Remove(value);
				if (val.get_Count() > 0)
				{
					return val.get_Item(0) as PeopleEvent;
				}
			}
			return null;
		}
	}
}
