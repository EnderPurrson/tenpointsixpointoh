using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class SubscriptionExpiredEvent : Jsonable
	{
		public string Sku
		{
			get;
			set;
		}

		public SubscriptionExpiredEvent()
		{
		}

		public static SubscriptionExpiredEvent CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SubscriptionExpiredEvent subscriptionExpiredEvent;
			try
			{
				if (jsonMap != null)
				{
					SubscriptionExpiredEvent item = new SubscriptionExpiredEvent();
					if (jsonMap.ContainsKey("sku"))
					{
						item.Sku = (string)jsonMap["sku"];
					}
					subscriptionExpiredEvent = item;
				}
				else
				{
					subscriptionExpiredEvent = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return subscriptionExpiredEvent;
		}

		public static SubscriptionExpiredEvent CreateFromJson(string jsonMessage)
		{
			SubscriptionExpiredEvent subscriptionExpiredEvent;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				subscriptionExpiredEvent = SubscriptionExpiredEvent.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return subscriptionExpiredEvent;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "sku", this.Sku }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<SubscriptionExpiredEvent> ListFromJson(List<object> array)
		{
			List<SubscriptionExpiredEvent> subscriptionExpiredEvents = new List<SubscriptionExpiredEvent>();
			foreach (object obj in array)
			{
				subscriptionExpiredEvents.Add(SubscriptionExpiredEvent.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return subscriptionExpiredEvents;
		}

		public static Dictionary<string, SubscriptionExpiredEvent> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SubscriptionExpiredEvent> strs = new Dictionary<string, SubscriptionExpiredEvent>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SubscriptionExpiredEvent subscriptionExpiredEvent = SubscriptionExpiredEvent.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, subscriptionExpiredEvent);
			}
			return strs;
		}

		public string ToJson()
		{
			string str;
			try
			{
				str = Json.Serialize(this.GetObjectDictionary());
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while Jsoning", applicationException);
			}
			return str;
		}
	}
}