using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated
{
	internal sealed class InAppPurchase : AggregatedEvent
	{
		private static readonly string PURCHASE_ID = "purchaseId";

		private static readonly string PURCHASE_TYPE = "purchaseType";

		private static readonly string PURCHASE_AMOUNT = "purchaseAmount";

		private static readonly string PURCHASE_PRICE = "purchasePrice";

		private static readonly string PURCHASE_CURRENCY = "purchasePriceCurrency";

		private List<InAppPurchaseData> aggregatedInGamePurchaseDatas;

		private readonly InAppPurchaseData data;

		public List<InAppPurchaseData> AggregatedInGamePurchaseDatas
		{
			get
			{
				return aggregatedInGamePurchaseDatas;
			}
			set
			{
				aggregatedInGamePurchaseDatas = value;
			}
		}

		public InAppPurchase()
		{
		}

		public InAppPurchase(ObjectInfo info)
			: base(info)
		{
			try
			{
				aggregatedInGamePurchaseDatas = info.GetValue("aggregatedInGamePurchaseDatas", typeof(List<InAppPurchaseData>)) as List<InAppPurchaseData>;
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
				info.AddValue("aggregatedInGamePurchaseDatas", aggregatedInGamePurchaseDatas);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, float purchasePrice, string purchasePriceCurrency)
			: base(EventType.InAppPurchase)
		{
			data = new InAppPurchaseData(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchasePriceCurrency);
			aggregatedInGamePurchaseDatas = new List<InAppPurchaseData>();
			SelfAggregate();
		}

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			JSONArray jSONArray = new JSONArray();
			Enumerator<InAppPurchaseData> enumerator = aggregatedInGamePurchaseDatas.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					InAppPurchaseData current = enumerator.get_Current();
					JSONClass jSONClass = new JSONClass();
					jSONArray.Add(jSONClass);
					jSONClass.Add(PURCHASE_ID, Uri.EscapeDataString(current.PurchaseId));
					jSONClass.Add(PURCHASE_TYPE, Uri.EscapeDataString(current.PurchaseType));
					jSONClass.Add(PURCHASE_AMOUNT, new JSONData(current.PurchaseAmount));
					jSONClass.Add(PURCHASE_PRICE, new JSONData(current.PurchasePrice));
					jSONClass.Add(PURCHASE_CURRENCY, Uri.EscapeDataString(current.PurchasePriceCurrency));
					jSONClass.Add(Event.TIMESTAMP, new JSONData(current.TimeStamp));
					AddPendingToJSON(jSONClass, current.PendingData);
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
			if (metric != null)
			{
				List<InAppPurchaseData> val = ((InAppPurchase)metric).aggregatedInGamePurchaseDatas;
				aggregatedInGamePurchaseDatas.AddRange((global::System.Collections.Generic.IEnumerable<InAppPurchaseData>)val);
			}
		}

		public override void AddPendingEvents(List<string> events)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<InAppPurchaseData> enumerator = aggregatedInGamePurchaseDatas.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					InAppPurchaseData current = enumerator.get_Current();
					current.PendingData = events;
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		protected override void SelfAggregate()
		{
			aggregatedInGamePurchaseDatas = new List<InAppPurchaseData>();
			aggregatedInGamePurchaseDatas.Add(data);
		}

		public override bool IsReadyToSend()
		{
			return true;
		}

		public override bool IsNeedToClear()
		{
			return true;
		}

		public override bool IsEqualToMetric(Event other)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			InAppPurchase inAppPurchase = other as InAppPurchase;
			if (data == null || inAppPurchase.data == null)
			{
				return false;
			}
			return data.PurchaseId.Equals(inAppPurchase.data.PurchaseId);
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
