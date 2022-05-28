using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics.Aggregated;

namespace DevToDev.Data.Metrics.Simple
{
	internal sealed class RealPayment : AggregatedEvent
	{
		private static readonly string NAME = "name";

		private static readonly string ENTRIES = "entries";

		private static readonly string ORDER_ID = "orderId";

		private static readonly string CURRENCY_CODE = "currencyCode";

		private static readonly string PRICE = "price";

		private Dictionary<string, List<RealPaymentData>> aggregatedRealPaymentDatas;

		private readonly RealPaymentData data;

		public Dictionary<string, List<RealPaymentData>> AggregatedRealPaymentDatas
		{
			get
			{
				return aggregatedRealPaymentDatas;
			}
			set
			{
				aggregatedRealPaymentDatas = value;
			}
		}

		public RealPayment()
		{
		}

		public RealPayment(string orderId, float price, string inAppName, string currencyCode)
			: base(EventType.RealPayment)
		{
			data = new RealPaymentData(orderId, price, inAppName, currencyCode);
			aggregatedRealPaymentDatas = new Dictionary<string, List<RealPaymentData>>();
			SelfAggregate();
		}

		public RealPayment(ObjectInfo info)
			: base(info)
		{
			try
			{
				aggregatedRealPaymentDatas = info.GetValue("aggregatedRealPaymentDatas", typeof(Dictionary<string, List<RealPaymentData>>)) as Dictionary<string, List<RealPaymentData>>;
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
				info.AddValue("aggregatedRealPaymentDatas", aggregatedRealPaymentDatas);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
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
			Enumerator<string, List<RealPaymentData>> enumerator = aggregatedRealPaymentDatas.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Enumerator<RealPaymentData> enumerator2 = enumerator.get_Current().get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							RealPaymentData current = enumerator2.get_Current();
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

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			JSONArray jSONArray = new JSONArray();
			Enumerator<string, List<RealPaymentData>> enumerator = aggregatedRealPaymentDatas.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<RealPaymentData>> current = enumerator.get_Current();
					JSONClass jSONClass = new JSONClass();
					jSONArray.Add(jSONClass);
					JSONArray jSONArray2 = new JSONArray();
					jSONClass.Add(ENTRIES, jSONArray2);
					jSONClass.Add(NAME, Uri.EscapeDataString(current.get_Key()));
					Enumerator<RealPaymentData> enumerator2 = current.get_Value().GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							RealPaymentData current2 = enumerator2.get_Current();
							JSONClass jSONClass2 = new JSONClass();
							jSONArray2.Add(jSONClass2);
							jSONClass2.Add(ORDER_ID, Uri.EscapeDataString(current2.OrderId));
							jSONClass2.Add(CURRENCY_CODE, current2.CurrencyCode);
							jSONClass2.Add(PRICE, new JSONData(current2.Price));
							jSONClass2.Add(Event.TIMESTAMP, new JSONData(current2.TimeStamp));
							AddPendingToJSON(jSONClass2, current2.PendingData);
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (metric == null)
			{
				return;
			}
			Dictionary<string, List<RealPaymentData>> val = (metric as RealPayment).aggregatedRealPaymentDatas;
			Enumerator<string, List<RealPaymentData>> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<RealPaymentData>> current = enumerator.get_Current();
					if (aggregatedRealPaymentDatas.ContainsKey(current.get_Key()))
					{
						aggregatedRealPaymentDatas.get_Item(current.get_Key()).AddRange((global::System.Collections.Generic.IEnumerable<RealPaymentData>)current.get_Value());
					}
					else
					{
						aggregatedRealPaymentDatas.Add(current.get_Key(), current.get_Value());
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		protected override void SelfAggregate()
		{
			aggregatedRealPaymentDatas = new Dictionary<string, List<RealPaymentData>>();
			Dictionary<string, List<RealPaymentData>> obj = aggregatedRealPaymentDatas;
			string inAppName = data.InAppName;
			List<RealPaymentData> val = new List<RealPaymentData>();
			val.Add(data);
			obj.Add(inAppName, val);
		}

		public override bool IsReadyToSend()
		{
			return true;
		}

		public override bool IsNeedToClear()
		{
			return true;
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
