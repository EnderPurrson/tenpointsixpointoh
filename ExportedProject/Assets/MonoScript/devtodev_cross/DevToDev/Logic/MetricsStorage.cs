using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Specific;

namespace DevToDev.Logic
{
	internal class MetricsStorage : ISaveable
	{
		private Dictionary<string, UserMetrics> userMetrics;

		public int Size
		{
			get
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				int num = 0;
				Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						num += enumerator.get_Current().get_Value().Size;
					}
					return num;
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
		}

		public MetricsStorage()
		{
			userMetrics = new Dictionary<string, UserMetrics>();
		}

		public MetricsStorage(ObjectInfo info)
		{
			try
			{
				userMetrics = info.GetValue("userMetrics", typeof(Dictionary<string, UserMetrics>)) as Dictionary<string, UserMetrics>;
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
				info.AddValue("userMetrics", userMetrics);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public MetricsStorage(int level)
			: this()
		{
			setLevel(level, null, false);
		}

		public MetricsStorage(int level, MetricsStorage oldData)
			: this(level)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (oldData == null)
			{
				return;
			}
			ProgressionEvent progressionEvent = null;
			Enumerator<string, UserMetrics> enumerator = oldData.userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() && (progressionEvent = enumerator.get_Current().get_Value().GetProgressionEvent()) == null)
				{
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			if (progressionEvent != null)
			{
				progressionEvent = new ProgressionEvent(progressionEvent);
				progressionEvent.RemoveSentMetrics();
			}
			Enumerator<string, UserMetrics> enumerator2 = oldData.userMetrics.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, UserMetrics> current = enumerator2.get_Current();
					if (current.get_Value().LevelData.Level > level)
					{
						current.get_Value().clearMetrics();
						if (userMetrics.ContainsKey(current.get_Key()))
						{
							userMetrics.set_Item(current.get_Key(), current.get_Value());
						}
						else
						{
							userMetrics.Add(current.get_Key(), current.get_Value());
						}
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			if (progressionEvent != null)
			{
				AddMetric(level, progressionEvent);
			}
		}

		public void AddPeopleEvent(int level, PeopleEvent peopleEvent)
		{
			AddMetric(level, peopleEvent);
		}

		public void ClearUnfinishedProgressionEvent()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().get_Value().ClearUnfinishedProgressionEvent();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public PeopleEvent GetRemovePeopleEvent()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			PeopleEvent peopleEvent = null;
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					peopleEvent = enumerator.get_Current().get_Value().GetRemovePeopleEvent();
					if (peopleEvent != null)
					{
						return peopleEvent;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return null;
		}

		public void AddMetric(int level, Event metric)
		{
			if (!(metric is ProgressionEvent))
			{
				Log.R("Metric {0} added to storage", metric.MetricName);
			}
			string text = level.ToString();
			if (!userMetrics.ContainsKey(text))
			{
				userMetrics.Add(text, new UserMetrics(level, false, null));
			}
			userMetrics.get_Item(text).addMetric(metric);
		}

		public void RemoveAllMetricsByType(EventType type)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().get_Value().RemoveAppMetricsByType(type);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public bool IsMetricExist(string metricCode)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().get_Value().isMetricExist(metricCode))
					{
						return true;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public string PrepareToSend()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			JSONClass jSONClass = new JSONClass();
			try
			{
				int num = 0;
				Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						num = Math.Max(num, int.Parse(enumerator.get_Current().get_Key()));
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				Enumerator<string, UserMetrics> enumerator2 = userMetrics.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, UserMetrics> current = enumerator2.get_Current();
						JSONNode jSONNode = current.get_Value().DataToSend(num);
						if (!(jSONNode == null))
						{
							jSONClass.Add(current.get_Key(), jSONNode);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator2).Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				Log.E(ex.get_Message() + "\n" + ex.get_StackTrace());
			}
			if (jSONClass.Count == 0)
			{
				return null;
			}
			return jSONClass.ToJSON(0);
		}

		private ProgressionEvent getProgressionEventMetric()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			ProgressionEvent result = null;
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if ((result = enumerator.get_Current().get_Value().GetRemoveProgressionEvent()) != null)
					{
						return result;
					}
				}
				return result;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public void setLevel(int level, Dictionary<string, int> resources, bool isNew)
		{
			string text = level.ToString();
			string text2 = (level + 1).ToString();
			ProgressionEvent progressionEventMetric = getProgressionEventMetric();
			if (!userMetrics.ContainsKey(text))
			{
				userMetrics.Add(text, new UserMetrics(level, isNew, resources));
			}
			else
			{
				userMetrics.get_Item(text).LevelData.IsNew = isNew;
				userMetrics.get_Item(text).LevelData.Balance = resources;
			}
			if (progressionEventMetric != null)
			{
				userMetrics.get_Item(text).addMetric(progressionEventMetric);
			}
			if (!userMetrics.ContainsKey(text2))
			{
				userMetrics.Add(text2, new UserMetrics(level + 1, false, null));
			}
		}

		public void upSpend(int level, int purchasePrice, string purchaseCurrency)
		{
			string text = (level + 1).ToString();
			if (userMetrics.ContainsKey(text))
			{
				userMetrics.get_Item(text).upSpend(purchaseCurrency, purchasePrice);
			}
		}

		public void upBought(int level, int purchasePrice, string purchaseCurrency)
		{
			string text = (level + 1).ToString();
			if (userMetrics.ContainsKey(text))
			{
				userMetrics.get_Item(text).upBought(purchaseCurrency, purchasePrice);
			}
		}

		public void upEarned(int level, int purchasePrice, string purchaseCurrency)
		{
			string text = (level + 1).ToString();
			if (userMetrics.ContainsKey(text))
			{
				userMetrics.get_Item(text).upEarned(purchaseCurrency, purchasePrice);
			}
		}

		public void ClearLevelData()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, UserMetrics> enumerator = userMetrics.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().get_Value().ClearLevelData();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}
}
