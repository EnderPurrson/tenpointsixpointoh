using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponse : Jsonable
	{
		public com.amazon.device.iap.cpt.AmazonUserData AmazonUserData
		{
			get;
			set;
		}

		public com.amazon.device.iap.cpt.PurchaseReceipt PurchaseReceipt
		{
			get;
			set;
		}

		public string RequestId
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public PurchaseResponse()
		{
		}

		public static PurchaseResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			PurchaseResponse purchaseResponse;
			try
			{
				if (jsonMap != null)
				{
					PurchaseResponse item = new PurchaseResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						item.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						item.AmazonUserData = com.amazon.device.iap.cpt.AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("purchaseReceipt"))
					{
						item.PurchaseReceipt = com.amazon.device.iap.cpt.PurchaseReceipt.CreateFromDictionary(jsonMap["purchaseReceipt"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						item.Status = (string)jsonMap["status"];
					}
					purchaseResponse = item;
				}
				else
				{
					purchaseResponse = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return purchaseResponse;
		}

		public static PurchaseResponse CreateFromJson(string jsonMessage)
		{
			PurchaseResponse purchaseResponse;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				purchaseResponse = PurchaseResponse.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return purchaseResponse;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			object objectDictionary;
			object obj;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "requestId", this.RequestId }
				};
				Dictionary<string, object> strs2 = strs1;
				if (this.AmazonUserData == null)
				{
					objectDictionary = null;
				}
				else
				{
					objectDictionary = this.AmazonUserData.GetObjectDictionary();
				}
				strs2.Add("amazonUserData", objectDictionary);
				Dictionary<string, object> strs3 = strs1;
				if (this.PurchaseReceipt == null)
				{
					obj = null;
				}
				else
				{
					obj = this.PurchaseReceipt.GetObjectDictionary();
				}
				strs3.Add("purchaseReceipt", obj);
				strs1.Add("status", this.Status);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<PurchaseResponse> ListFromJson(List<object> array)
		{
			List<PurchaseResponse> purchaseResponses = new List<PurchaseResponse>();
			foreach (object obj in array)
			{
				purchaseResponses.Add(PurchaseResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return purchaseResponses;
		}

		public static Dictionary<string, PurchaseResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseResponse> strs = new Dictionary<string, PurchaseResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				PurchaseResponse purchaseResponse = PurchaseResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, purchaseResponse);
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