using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class RequestOutput : Jsonable
	{
		public string RequestId
		{
			get;
			set;
		}

		public RequestOutput()
		{
		}

		public static RequestOutput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			RequestOutput requestOutput;
			try
			{
				if (jsonMap != null)
				{
					RequestOutput item = new RequestOutput();
					if (jsonMap.ContainsKey("requestId"))
					{
						item.RequestId = (string)jsonMap["requestId"];
					}
					requestOutput = item;
				}
				else
				{
					requestOutput = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return requestOutput;
		}

		public static RequestOutput CreateFromJson(string jsonMessage)
		{
			RequestOutput requestOutput;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				requestOutput = RequestOutput.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return requestOutput;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "requestId", this.RequestId }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<RequestOutput> ListFromJson(List<object> array)
		{
			List<RequestOutput> requestOutputs = new List<RequestOutput>();
			foreach (object obj in array)
			{
				requestOutputs.Add(RequestOutput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return requestOutputs;
		}

		public static Dictionary<string, RequestOutput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, RequestOutput> strs = new Dictionary<string, RequestOutput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				RequestOutput requestOutput = RequestOutput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, requestOutput);
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