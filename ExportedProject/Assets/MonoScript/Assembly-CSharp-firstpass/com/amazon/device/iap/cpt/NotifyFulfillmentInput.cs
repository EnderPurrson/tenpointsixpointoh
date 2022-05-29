using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class NotifyFulfillmentInput : Jsonable
	{
		public string FulfillmentResult
		{
			get;
			set;
		}

		public string ReceiptId
		{
			get;
			set;
		}

		public NotifyFulfillmentInput()
		{
		}

		public static NotifyFulfillmentInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			NotifyFulfillmentInput notifyFulfillmentInput;
			try
			{
				if (jsonMap != null)
				{
					NotifyFulfillmentInput item = new NotifyFulfillmentInput();
					if (jsonMap.ContainsKey("receiptId"))
					{
						item.ReceiptId = (string)jsonMap["receiptId"];
					}
					if (jsonMap.ContainsKey("fulfillmentResult"))
					{
						item.FulfillmentResult = (string)jsonMap["fulfillmentResult"];
					}
					notifyFulfillmentInput = item;
				}
				else
				{
					notifyFulfillmentInput = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return notifyFulfillmentInput;
		}

		public static NotifyFulfillmentInput CreateFromJson(string jsonMessage)
		{
			NotifyFulfillmentInput notifyFulfillmentInput;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				notifyFulfillmentInput = NotifyFulfillmentInput.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return notifyFulfillmentInput;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "receiptId", this.ReceiptId },
					{ "fulfillmentResult", this.FulfillmentResult }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<NotifyFulfillmentInput> ListFromJson(List<object> array)
		{
			List<NotifyFulfillmentInput> notifyFulfillmentInputs = new List<NotifyFulfillmentInput>();
			foreach (object obj in array)
			{
				notifyFulfillmentInputs.Add(NotifyFulfillmentInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return notifyFulfillmentInputs;
		}

		public static Dictionary<string, NotifyFulfillmentInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, NotifyFulfillmentInput> strs = new Dictionary<string, NotifyFulfillmentInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				NotifyFulfillmentInput notifyFulfillmentInput = NotifyFulfillmentInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, notifyFulfillmentInput);
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