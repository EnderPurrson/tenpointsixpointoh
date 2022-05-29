using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetPurchaseUpdatesResponse : Jsonable
	{
		public com.amazon.device.iap.cpt.AmazonUserData AmazonUserData
		{
			get;
			set;
		}

		public bool HasMore
		{
			get;
			set;
		}

		public List<PurchaseReceipt> Receipts
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

		public GetPurchaseUpdatesResponse()
		{
		}

		public static GetPurchaseUpdatesResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetPurchaseUpdatesResponse getPurchaseUpdatesResponse;
			try
			{
				if (jsonMap != null)
				{
					GetPurchaseUpdatesResponse item = new GetPurchaseUpdatesResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						item.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						item.AmazonUserData = com.amazon.device.iap.cpt.AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("receipts"))
					{
						item.Receipts = PurchaseReceipt.ListFromJson(jsonMap["receipts"] as List<object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						item.Status = (string)jsonMap["status"];
					}
					if (jsonMap.ContainsKey("hasMore"))
					{
						item.HasMore = (bool)jsonMap["hasMore"];
					}
					getPurchaseUpdatesResponse = item;
				}
				else
				{
					getPurchaseUpdatesResponse = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return getPurchaseUpdatesResponse;
		}

		public static GetPurchaseUpdatesResponse CreateFromJson(string jsonMessage)
		{
			GetPurchaseUpdatesResponse getPurchaseUpdatesResponse;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				getPurchaseUpdatesResponse = GetPurchaseUpdatesResponse.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return getPurchaseUpdatesResponse;
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
				if (this.Receipts == null)
				{
					obj = null;
				}
				else
				{
					obj = Jsonable.unrollObjectIntoList<PurchaseReceipt>(this.Receipts);
				}
				strs3.Add("receipts", obj);
				strs1.Add("status", this.Status);
				strs1.Add("hasMore", this.HasMore);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<GetPurchaseUpdatesResponse> ListFromJson(List<object> array)
		{
			List<GetPurchaseUpdatesResponse> getPurchaseUpdatesResponses = new List<GetPurchaseUpdatesResponse>();
			foreach (object obj in array)
			{
				getPurchaseUpdatesResponses.Add(GetPurchaseUpdatesResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return getPurchaseUpdatesResponses;
		}

		public static Dictionary<string, GetPurchaseUpdatesResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetPurchaseUpdatesResponse> strs = new Dictionary<string, GetPurchaseUpdatesResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetPurchaseUpdatesResponse getPurchaseUpdatesResponse = GetPurchaseUpdatesResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, getPurchaseUpdatesResponse);
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