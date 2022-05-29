using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseReceipt : Jsonable
	{
		public long CancelDate
		{
			get;
			set;
		}

		public string ProductType
		{
			get;
			set;
		}

		public long PurchaseDate
		{
			get;
			set;
		}

		public string ReceiptId
		{
			get;
			set;
		}

		public string Sku
		{
			get;
			set;
		}

		public PurchaseReceipt()
		{
		}

		public static PurchaseReceipt CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			PurchaseReceipt purchaseReceipt;
			try
			{
				if (jsonMap != null)
				{
					PurchaseReceipt item = new PurchaseReceipt();
					if (jsonMap.ContainsKey("receiptId"))
					{
						item.ReceiptId = (string)jsonMap["receiptId"];
					}
					if (jsonMap.ContainsKey("cancelDate"))
					{
						item.CancelDate = (long)jsonMap["cancelDate"];
					}
					if (jsonMap.ContainsKey("purchaseDate"))
					{
						item.PurchaseDate = (long)jsonMap["purchaseDate"];
					}
					if (jsonMap.ContainsKey("sku"))
					{
						item.Sku = (string)jsonMap["sku"];
					}
					if (jsonMap.ContainsKey("productType"))
					{
						item.ProductType = (string)jsonMap["productType"];
					}
					purchaseReceipt = item;
				}
				else
				{
					purchaseReceipt = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return purchaseReceipt;
		}

		public static PurchaseReceipt CreateFromJson(string jsonMessage)
		{
			PurchaseReceipt purchaseReceipt;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				purchaseReceipt = PurchaseReceipt.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return purchaseReceipt;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "receiptId", this.ReceiptId },
					{ "cancelDate", this.CancelDate },
					{ "purchaseDate", this.PurchaseDate },
					{ "sku", this.Sku },
					{ "productType", this.ProductType }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<PurchaseReceipt> ListFromJson(List<object> array)
		{
			List<PurchaseReceipt> purchaseReceipts = new List<PurchaseReceipt>();
			foreach (object obj in array)
			{
				purchaseReceipts.Add(PurchaseReceipt.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return purchaseReceipts;
		}

		public static Dictionary<string, PurchaseReceipt> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseReceipt> strs = new Dictionary<string, PurchaseReceipt>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				PurchaseReceipt purchaseReceipt = PurchaseReceipt.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, purchaseReceipt);
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